using Blog_System.Data;
using Blog_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Blog_System.Controllers
{
    // Controllers/PostController.cs
    public class PostController : Controller
    {
        private BlogContext _context;

        public PostController(BlogContext context)
        {
            _context = context;
        }

        // GET: Post
        public ActionResult Index()
        {
            return View(_context.Posts.ToList());
        }

        // GET: Post/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }
            var post = _context.Posts.Find(id);
            if (post == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.NotFound);
            }
            return View(post);
        }

        // GET: Post/Create
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

        // GET: Post/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }
            Post post = _context.Posts.Find(id);
            if (post == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.NotFound);
            }
            return View(post);
        }

        // POST: Post/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(include: "Id,Title,Content,CreatedAt")] Post post)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(post).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Post/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }
            Post post = _context.Posts.Find(id);
            if (post == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.NotFound);
            }
            return View(post);
        }

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = _context.Posts.Find(id);
            _context.Posts.Remove(post);
            _context.SaveChanges();
            return RedirectToAction("Index");
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

}
