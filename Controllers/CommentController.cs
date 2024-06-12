using Blog_System.Data;
using Blog_System.Models;
using Blog_System.ViewModel;
using Microsoft.AspNetCore.Mvc;
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
                    Author = model.Author,
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
