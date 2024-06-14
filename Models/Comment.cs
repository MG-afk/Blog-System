namespace Blog_System.Models;

public class Comment
{
    public int Id { get; set; }
    public required string Author { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; }

    public int PostId { get; set; }
    public virtual Post? Post { get; set; }
}
