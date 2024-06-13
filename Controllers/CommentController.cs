using Blog_System.Data;
using Blog_System.Models;
using Blog_System.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog_System.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public sealed class CommentController : Controller
    {
        private readonly BlogContext _context;

        public CommentController(BlogContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PostCommentViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = new Comment
            {
                Author = model.Author,
                Content = model.Content,
                CreatedAt = DateTime.Now
            };

            var currentPost = await _context.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == model.PostId);

            if (currentPost == null)
                return NotFound("You can't add a comment to a non-existing post");

            currentPost.Comments ??= new List<Comment>();

            currentPost.Comments.Add(comment);

            _context.Comments.Add(comment);

            await _context.SaveChangesAsync();

            return PartialView("_CommentPartial", comment);
        }
    }
}
