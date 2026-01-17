namespace LinkedInLite.Api.Domain.Entities;

public class Reaction
{
    public int Id { get; set; }

    public int PostId { get; set; }
    public Post Post { get; set; } = null!;

    public int ProfileId { get; set; }
    public Profile Profile { get; set; } = null!;
}
