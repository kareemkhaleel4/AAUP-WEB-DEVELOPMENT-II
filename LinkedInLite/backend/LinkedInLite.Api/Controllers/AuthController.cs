using LinkedInLite.Api.Data;
using LinkedInLite.Api.Domain.Entities;
using LinkedInLite.Api.Dtos.Auth;
using LinkedInLite.Api.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LinkedInLite.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly JwtService _jwt;

    public AuthController(AppDbContext db, JwtService jwt)
    {
        _db = db;
        _jwt = jwt;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (await _db.Users.AnyAsync(u => u.Email == request.Email))
            return BadRequest("Email already exists");

        var user = new User
        {
            Email = request.Email,
            PasswordHash = PasswordHasher.Hash(request.Password)
        };

        var profile = new Profile
        {
            FullName = request.FullName,
            Headline = "New professional",
            Location = "",
            Summary = "",
            User = user
        };

        _db.Users.Add(user);
        _db.Profiles.Add(profile);

        await _db.SaveChangesAsync();

        var token = _jwt.GenerateToken(user.Id, user.Email);

        return Ok(new AuthResponse { Token = token });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var user = await _db.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !PasswordHasher.Verify(request.Password, user.PasswordHash))
            return Unauthorized();

        var token = _jwt.GenerateToken(user.Id, user.Email);

        return Ok(new AuthResponse
        {
            Token = token,
            FullName = user.Profile.FullName,
            ProfileImageUrl = user.Profile.ProfileImageUrl
        });
    }
}
