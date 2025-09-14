using WorkFound.Domain.Entities.Jobs.Application.Questions;

namespace WorkFound.Domain.Entities.Jobs.Application.Answers;

public class JobAppSubmitAnswerOption
{
    public Guid Id { get; set; }
    
    public Guid AnswerId { get; set; }
    public JobAppSubmitAnswer Answer { get; set; }

    public Guid OptionId { get; set; }
    public JobApplicationQuestionOption Option { get; set; }
}