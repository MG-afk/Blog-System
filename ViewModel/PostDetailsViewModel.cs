using Blog_System.Models;

namespace Blog_System.ViewModel;

public class PostDetailsViewModel
{
    public Post Post { get; set; }
    public Comment NewComment { get; set; }
}
