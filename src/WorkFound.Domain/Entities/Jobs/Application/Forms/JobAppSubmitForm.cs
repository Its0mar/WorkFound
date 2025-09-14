using WorkFound.Domain.Entities.Jobs.Application.Answers;
using WorkFound.Domain.Entities.Profile.User;

namespace WorkFound.Domain.Entities.Jobs.Application.Forms;

public class JobAppSubmitForm
{
    public Guid Id { get; set; }
    
    public Guid JobApplicationId { get; set; }
    public JobApplicationForm? JobApplication { get; set; }
    
    public Guid UserProfileId { get; set; }
    public UserProfile? UserProfile { get; set; }
    
    public string? CoverLetter { get; set; }
    public string? ResumeUrl { get; set; }
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public List<JobAppSubmitAnswer> Answers { get; set; } = new();
    
}