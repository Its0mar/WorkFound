using System.ComponentModel.DataAnnotations;

namespace WorkFound.Application.Jobs.Dto.Application.Apply;

public record CreateJobApplicationFormDto
{
    public required Guid JobId { get; init; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    
    [Required]
    [MinLength(1, ErrorMessage = "At least one question is required.")]
    public List<CreateJobApplicationQuestionDto> Questions { get; set; } = new();

}