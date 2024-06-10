using Blog_System.Data;
using Blog_System.Models;
using Blog_System.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Blog_System.Controllers;

[Route("[controller]/[action]")]
public sealed class CommentController(BlogContext context) : Controller
{
    private readonly BlogContext _context = context;

    public async Task<IActionResult> Index()
    {
        var comments = await _context.Comments.Include(c => c.Post).ToListAsync();
        return View(comments);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var comment = await _context.Comments
            .Include(c => c.Post)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (comment == null)
            return NotFound();

        return View(comment);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return NotFound();
        }
        ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Title", comment.PostId);
        return View(comment);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,PostId,Content,CreatedAt")] Comment comment)
    {
        if (id != comment.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(comment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(comment.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Title", comment.PostId);
        return View(comment);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var comment = await _context.Comments
            .Include(c => c.Post)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (comment == null)
        {
            return NotFound();
        }

        return View(comment);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CommentExists(int id)
    {
        return _context.Comments.Any(e => e.Id == id);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CommentViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var comment = new Comment
                {
                    PostId = model.PostId,
                    Content = model.Content,
                    CreatedAt = DateTime.Now
                };

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                return PartialView("_CommentPartial", comment);
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        else
        {
            return BadRequest(ModelState);
        }
    }
}
