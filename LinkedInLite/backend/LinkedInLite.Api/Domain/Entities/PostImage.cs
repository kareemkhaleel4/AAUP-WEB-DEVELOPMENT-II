namespace LinkedInLite.Api.Domain.Entities;

public class PostImage
{
    public int Id { get; set; }

    public int PostId { get; set; }
    public Post Post { get; set; } = null!;

    public string FileName { get; set; } = null!;
    public string Url { get; set; } = null!;
}
