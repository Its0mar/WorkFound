using WorkFound.Domain.Entities.Auth;

namespace WorkFound.Domain.Entities.Profile;

public class UserSkills
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public required AppUser AppUser { get; set; }
    public required string Title { get; set; }
}