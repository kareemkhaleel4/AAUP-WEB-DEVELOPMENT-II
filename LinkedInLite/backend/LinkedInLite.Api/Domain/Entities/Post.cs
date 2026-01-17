namespace LinkedInLite.Api.Domain.Entities;

public class Post
{
    public int Id { get; set; }

    public int AuthorProfileId { get; set; }
    public Profile AuthorProfile { get; set; } = null!;

    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation
    public ICollection<PostImage> Images { get; set; } = new List<PostImage>();
    public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
