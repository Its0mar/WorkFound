
namespace WorkFound.Domain.Entities.Profile.User;

public class UserSkill
{
    public Guid Id { get; set; }
    public required string SkillName { get; set; }
    
    public required Guid UserProfileId { get; set; }
    public UserProfile? UserProfile { get; set; }
}