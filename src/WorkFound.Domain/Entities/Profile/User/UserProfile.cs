using WorkFound.Domain.Entities.Auth;
using WorkFound.Domain.Entities.Common;
using WorkFound.Domain.Entities.Jobs.Application.Forms;

namespace WorkFound.Domain.Entities.Profile.User;

public class UserProfile
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public Auth.AppUser? AppUser { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Location { get; set; }
    public string? Bio { get; set; }
    public string? ProfilePictureUrl { get; set; }
        
    public List<UserExperience> UserExperiences { get; set; } = new();
    public List<UserEducation> UserEducations { get; set; } = new();
    // public List<UserSkill> UserSkills { get; set; } = new();
    public List<Skill> Skills { get; set; } = new();
    public List<JobAppSubmitForm> JobAppSubmitForms { get; set; } = new();
    public string Name => $"{FirstName} {LastName}";
}