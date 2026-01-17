using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LinkedInLite.Api.Dtos.Posts;

public class CreatePostRequest
{
    [Required]
    public string Content { get; set; } = null!;

    // Optional images
    public List<IFormFile>? Images { get; set; }
}
