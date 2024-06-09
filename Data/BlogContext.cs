using Blog_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog_System.Data
{
    public class BlogContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public BlogContext(DbContextOptions<BlogContext> options) : base(options) { }
    }
}
