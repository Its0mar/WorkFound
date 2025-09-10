using WorkFound.Domain.Entities.Enums;
using WorkFound.Domain.Entities.Jobs.Application.Forms;

namespace WorkFound.Domain.Entities.Jobs.Application.Questions;

public class JobApplicationQuestion
{
    public Guid Id { get; set; }
    
    public Guid JobApplicationFormId { get; set; }
    public JobApplicationForm? JobApplicationForm { get; set; }
    
    public required string Text { get; set; } 
    public required int Order { get; set; }
    public required bool IsRequired { get; set; }
    
    public QuestionType QuestionType { get; set; }
    public List<JobApplicationQuestionOption>? Options { get; set; } 
}

