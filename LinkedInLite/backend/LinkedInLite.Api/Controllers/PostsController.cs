using LinkedInLite.Api.Data;
using LinkedInLite.Api.Domain.Entities;
using LinkedInLite.Api.Dtos.Posts;
using LinkedInLite.Api.Services.FileStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LinkedInLite.Api.Controllers;

[ApiController]
[Route("api/posts")]
[Authorize]
public class PostsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IFileStorage _fileStorage;

    public PostsController(AppDbContext db, IFileStorage fileStorage)
    {
        _db = db;
        _fileStorage = fileStorage;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromForm] CreatePostRequest request)
    {
        var userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var profile = await _db.Profiles
            .FirstAsync(p => p.UserId == userId);

        var post = new Post
        {
            Content = request.Content,
            AuthorProfileId = profile.Id
        };

        if (request.Images != null)
        {
            foreach (var image in request.Images)
            {
                var url = await _fileStorage.SavePostImageAsync(image);
                post.Images.Add(new PostImage
                {
                    FileName = image.FileName,
                    Url = url
                });
            }
        }

        _db.Posts.Add(post);
        await _db.SaveChangesAsync();

        return Ok();
    }
    [HttpGet]
    public async Task<ActionResult<List<PostFeedResponse>>> GetFeed()
    {
        var userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var posts = await _db.Posts
            .AsNoTracking()
            .Include(p => p.AuthorProfile)
            .Include(p => p.Images)
            .Include(p => p.Reactions)
            .Include(p => p.Comments)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new PostFeedResponse
            {
                PostId = p.Id,

                AuthorName = p.AuthorProfile.FullName,
                AuthorHeadline = p.AuthorProfile.Headline,
                AuthorImageUrl = p.AuthorProfile.ProfileImageUrl,

                Content = p.Content,
                ImageUrls = p.Images
                    .Select(i => i.Url)
                    .ToList(),

                LikesCount = p.Reactions.Count,
                CommentsCount = p.Comments.Count,

                CreatedAt = p.CreatedAt,

                IsOwner = p.AuthorProfile.UserId == userId
            })
            .ToListAsync();

        return Ok(posts);
    }

    [HttpDelete("{postId:int}")]
    public async Task<IActionResult> DeletePost(int postId)
    {
        var userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var profileId = await _db.Profiles
            .Where(p => p.UserId == userId)
            .Select(p => p.Id)
            .FirstAsync();

        var post = await _db.Posts
            .Include(p => p.Images)
            .Include(p => p.Comments)
            .Include(p => p.Reactions)
            .FirstOrDefaultAsync(p =>
                p.Id == postId &&
                p.AuthorProfileId == profileId
            );

        if (post == null)
            return NotFound();

        _db.Posts.Remove(post);
        await _db.SaveChangesAsync();

        return NoContent();
    }


}
