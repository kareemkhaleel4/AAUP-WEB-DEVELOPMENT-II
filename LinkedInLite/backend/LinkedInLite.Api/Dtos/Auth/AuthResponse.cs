namespace LinkedInLite.Api.Dtos.Auth;

public class AuthResponse
{
    public string Token { get; set; } = null!;
    
    public string FullName { get; set; } = null!;
    public string? ProfileImageUrl { get; set; }
}
