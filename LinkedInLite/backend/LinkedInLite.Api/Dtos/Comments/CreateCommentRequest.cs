using System.ComponentModel.DataAnnotations;

namespace LinkedInLite.Api.Dtos.Comments;

public class CreateCommentRequest
{
    [Required]
    public string Content { get; set; } = null!;
}
