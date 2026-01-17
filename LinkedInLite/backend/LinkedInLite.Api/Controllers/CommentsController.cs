using LinkedInLite.Api.Data;
using LinkedInLite.Api.Domain.Entities;
using LinkedInLite.Api.Dtos.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LinkedInLite.Api.Controllers;

[ApiController]
[Route("api/posts/{postId}/comments")]
public class CommentsController : ControllerBase
{
    private readonly AppDbContext _db;

    public CommentsController(AppDbContext db)
    {
        _db = db;
    }

    // GET /api/posts/{postId}/comments
    [HttpGet]
    public async Task<ActionResult<List<CommentResponse>>> GetComments(int postId)
    {
        var userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var comments = await _db.Comments
            .AsNoTracking()
            .Where(c => c.PostId == postId)
            .Include(c => c.Profile)
            .OrderBy(c => c.CreatedAt)
            .Select(c => new CommentResponse
            {
                CommentId = c.Id,
                Content = c.Content,
                CreatedAt = c.CreatedAt,

                AuthorName = c.Profile.FullName,
                AuthorHeadline = c.Profile.Headline,
                AuthorImageUrl = c.Profile.ProfileImageUrl,

                IsOwner = c.Profile.UserId == userId
            })
            .ToListAsync();

        return Ok(comments);
    }
    // Add comment (requires login)
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddComment(
        int postId,
        CreateCommentRequest request)
    {
        var userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var profile = await _db.Profiles
            .FirstAsync(p => p.UserId == userId);

        var comment = new Comment
        {
            PostId = postId,
            ProfileId = profile.Id,
            Content = request.Content
        };

        _db.Comments.Add(comment);
        await _db.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{commentId:int}")]
    public async Task<IActionResult> DeleteComment(int postId, int commentId)
    {
        var userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var profileId = await _db.Profiles
            .Where(p => p.UserId == userId)
            .Select(p => p.Id)
            .FirstAsync();

        var comment = await _db.Comments
            .FirstOrDefaultAsync(c =>
                c.Id == commentId &&
                c.PostId == postId &&
                c.ProfileId == profileId
            );

        if (comment == null)
            return NotFound();

        _db.Comments.Remove(comment);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
