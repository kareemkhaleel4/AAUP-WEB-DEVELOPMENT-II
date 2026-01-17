namespace LinkedInLite.Api.Dtos.Profile;

public class ProfileResponse
{
    public int ProfileId { get; set; }
    public string FullName { get; set; } = null!;
    public string Headline { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public string? ProfileImageUrl { get; set; }
}
