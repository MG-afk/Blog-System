using Blog_System.Data;
using Blog_System.Models;
using Blog_System.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog_System.Controllers;

public sealed class PostController(BlogContext context) : Controller
{

    private readonly BlogContext _context = context;

    public async Task<ActionResult> Index()
    {
        var posts = await _context.Posts.ToListAsync();
        return View(posts);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var post = await _context.Posts
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (post == null)
        {
            return NotFound();
        }

        var viewModel = new PostDetailsViewModel
        {
            Post = post,
            NewComment = new Comment { PostId = post.Id }
        };

        return View(viewModel);
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Content")] Post post)
    {
        if (ModelState.IsValid)
        {
            post.CreatedAt = DateTime.Now;
            _context.Add(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(post);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var post = await _context.Posts.FindAsync(id);

        if (post == null)
            return NotFound();

        return View(post);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content")] Post post)
    {
        if (id != post.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(post);

        try
        {
            post.CreatedAt = DateTime.Now;
            _context.Update(post);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (PostExists(post.Id))
                throw;

            return NotFound();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool PostExists(int id)
    {
        return _context.Posts.Any(e => e.Id == id);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var post = await _context.Posts.FirstOrDefaultAsync(m => m.Id == id);

        if (post == null)
            return NotFound();

        return View(post);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var post = await _context.Posts.FindAsync(id);

        if (post == null)
            return NotFound();

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _context.Dispose();
        }
        base.Dispose(disposing);
    }
}
