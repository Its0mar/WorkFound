using WorkFound.Domain.Entities.Jobs.Application.Forms;
using WorkFound.Domain.Entities.Jobs.Application.Questions;

namespace WorkFound.Domain.Entities.Jobs.Application.Answers;

public class JobAppSubmitAnswer
{
    public Guid Id { get; set; }
    
    public Guid JobAppSubmitFormId { get; set; }
    public JobAppSubmitForm? JobAppSubmitForm { get; set; }
    
    public Guid JobApplicationQuestionId { get; set; }
    public JobApplicationQuestion? JobApplicationQuestion { get; set; }
    
    public string? AnswerText { get; set; }
    public List<JobAppSubmitAnswerOption>? SelectedOptions { get; set; }
}