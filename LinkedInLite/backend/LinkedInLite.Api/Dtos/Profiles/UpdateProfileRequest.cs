namespace LinkedInLite.Api.Dtos.Profile;

public class UpdateProfileRequest
{
    public string FullName { get; set; } = null!;
    public string Headline { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string Summary { get; set; } = null!;
}
