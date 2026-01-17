using LinkedInLite.Api.Data;
using LinkedInLite.Api.Domain.Entities;
using LinkedInLite.Api.Dtos.Reactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LinkedInLite.Api.Controllers;

[ApiController]
[Route("api/posts/{postId}/like")]
[Authorize]
public class ReactionsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ReactionsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<ActionResult<ReactionResponse>> LikePost(int postId)
    {
        var userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var profile = await _db.Profiles
            .FirstAsync(p => p.UserId == userId);

        var exists = await _db.Reactions
            .AnyAsync(r => r.PostId == postId && r.ProfileId == profile.Id);

        if (!exists)
        {
            _db.Reactions.Add(new Reaction
            {
                PostId = postId,
                ProfileId = profile.Id
            });

            await _db.SaveChangesAsync();
        }

        var count = await _db.Reactions
            .CountAsync(r => r.PostId == postId);

        return Ok(new ReactionResponse { LikesCount = count });
    }

    [HttpDelete]
    public async Task<ActionResult<ReactionResponse>> UnlikePost(int postId)
    {
        var userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var profile = await _db.Profiles
            .FirstAsync(p => p.UserId == userId);

        var reaction = await _db.Reactions
            .FirstOrDefaultAsync(r =>
                r.PostId == postId &&
                r.ProfileId == profile.Id);

        if (reaction != null)
        {
            _db.Reactions.Remove(reaction);
            await _db.SaveChangesAsync();
        }

        var count = await _db.Reactions
            .CountAsync(r => r.PostId == postId);

        return Ok(new ReactionResponse { LikesCount = count });
    }
}
