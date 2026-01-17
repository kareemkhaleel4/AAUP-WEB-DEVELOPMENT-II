namespace LinkedInLite.Api.Dtos.Posts;

public class PostFeedResponse
{
    public int PostId { get; set; }

    public string AuthorName { get; set; } = null!;
    public string AuthorHeadline { get; set; } = null!;
    public string? AuthorImageUrl { get; set; }

    public string Content { get; set; } = null!;
    public List<string> ImageUrls { get; set; } = new();

    public int LikesCount { get; set; }
    public int CommentsCount { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsOwner { get; set; }
}
