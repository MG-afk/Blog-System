using System.ComponentModel.DataAnnotations;

namespace Blog_System.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
    }
}
