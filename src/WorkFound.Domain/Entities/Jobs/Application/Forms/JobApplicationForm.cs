using WorkFound.Domain.Entities.Jobs.Application.Questions;

namespace WorkFound.Domain.Entities.Jobs.Application.Forms;

public class JobApplicationForm
{
    public Guid Id { get; set; }
    
    public Guid JobId { get; set; }
    public JobPost? Job { get; set; }
    
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? CoverLetterPrompt { get; set; }
    public string? ResumePrompt { get; set; }
    public string? ThankYouMessage { get; set; }
    public bool IsActive { get; set; } = true;
    public int Version { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DisabledAt { get; set; }
    
    public List<JobApplicationQuestion> Questions { get; set; } = new();
    public List<JobAppSubmitForm> Submissions { get; set; } = new();

    
}