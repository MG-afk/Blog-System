using Blog_System.Data;
using Blog_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog_System.Controllers;

[Route("[controller]/[action]")]
public sealed class CommentController(BlogContext context) : Controller
{
    private readonly BlogContext _context = context;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Comment model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

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
}
