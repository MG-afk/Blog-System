namespace Blog_System.Models;

public class Post
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Comment>? Comments { get; set; }
}
