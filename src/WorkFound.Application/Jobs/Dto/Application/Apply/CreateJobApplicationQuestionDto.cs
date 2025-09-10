using System.ComponentModel.DataAnnotations;

namespace WorkFound.Application.Jobs.Dto.Application.Apply;

public record CreateJobApplicationQuestionDto
{
    public required string Text { get; init; }
    [Range(1, int.MaxValue)]
    public required int Order { get; set; }
    public required bool IsRequired { get; set; }
    
    public QuestionTypeDto QuestionTypeDto { get; set; }
    public List<string>? Options { get; set; } 
}