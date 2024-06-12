using Blog_System.Data;
using Blog_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Blog_System.Controllers;

public sealed class HomeController(BlogContext context, ILogger<HomeController> logger) : Controller
{
    private readonly BlogContext _context = context;
    private readonly ILogger<HomeController> _logger = logger;

    public IActionResult Index()
    {
        return View(_context.Posts);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
