namespace WorkFound.Domain.Entities.Jobs.Application.Questions;

public class JobApplicationQuestionOption
{
    public Guid Id { get; set; }
    
    public Guid JobApplicationQuestionId { get; set; }
    public JobApplicationQuestion? JobApplicationQuestion { get; set; }
    
    public required string Text { get; set; }
    public required int Order { get; set; }
}