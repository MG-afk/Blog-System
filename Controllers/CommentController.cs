using Blog_System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Blog_System.Controllers;

public class CommentController : Controller
{
    private readonly BlogContext _context;

    public CommentController(BlogContext context)
    {
        _context = context;
    }


    // GET: Comment/Details/5
    public ActionResult Details(int? id)
    {
        if (id == null)
        {
            return new StatusCodeResult((int)HttpStatusCode.BadRequest);
        }

        // Include related entity (if needed)
        var comment = _context.Comments
            .Include(c => c.Post) // Example of including related entity
            .FirstOrDefault(c => c.Id == id);

        if (comment == null)
        {
            return new StatusCodeResult((int)HttpStatusCode.NotFound);
        }

        return View(comment);
    }

    // Other action methods...

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _context.Dispose();
        }
        base.Dispose(disposing);
    }
}
