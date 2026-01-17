using LinkedInLite.Api.Data;
using LinkedInLite.Api.Domain.Entities;
using LinkedInLite.Api.Dtos.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LinkedInLite.Api.Controllers;

[ApiController]
[Route("api/profile")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;

    public ProfileController(AppDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    private int CurrentUserId =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // GET /api/profile/me
    [HttpGet("me")]
    public async Task<ActionResult<ProfileResponse>> GetMyProfile()
    {
        var profile = await _db.Profiles
            .FirstAsync(p => p.UserId == CurrentUserId);

        return Ok(Map(profile));
    }

    // PUT /api/profile/me
    [HttpPut("me")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileRequest req)
    {
        var profile = await _db.Profiles
            .FirstAsync(p => p.UserId == CurrentUserId);

        profile.FullName = req.FullName;
        profile.Headline = req.Headline;
        profile.Location = req.Location;
        profile.Summary = req.Summary;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // POST /api/profile/me/image
    [HttpPost("me/image")]
    public async Task<ActionResult<string>> UploadProfileImage(IFormFile image)
    {
        if (image == null || image.Length == 0)
            return BadRequest("Invalid image");

        var folder = Path.Combine(_env.WebRootPath, "profile-images");
        Directory.CreateDirectory(folder);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
        var path = Path.Combine(folder, fileName);

        using var stream = new FileStream(path, FileMode.Create);
        await image.CopyToAsync(stream);

        var profile = await _db.Profiles
            .FirstAsync(p => p.UserId == CurrentUserId);

        profile.ProfileImageUrl = $"/profile-images/{fileName}";
        await _db.SaveChangesAsync();

        return Ok(profile.ProfileImageUrl);
    }

    private static ProfileResponse Map(Profile p) => new()
    {
        ProfileId = p.Id,
        FullName = p.FullName,
        Headline = p.Headline,
        Location = p.Location,
        Summary = p.Summary,
        ProfileImageUrl = p.ProfileImageUrl
    };
}
