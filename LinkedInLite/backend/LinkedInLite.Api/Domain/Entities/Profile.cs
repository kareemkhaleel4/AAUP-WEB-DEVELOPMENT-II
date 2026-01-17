namespace LinkedInLite.Api.Domain.Entities;

public class Profile
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public string FullName { get; set; } = null!;
    public string Headline { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string Summary { get; set; } = null!;

    public string? ProfileImageUrl { get; set; }

    // Navigation
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}
