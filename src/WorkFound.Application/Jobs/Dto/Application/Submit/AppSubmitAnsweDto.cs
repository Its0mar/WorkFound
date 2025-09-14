using System.ComponentModel.DataAnnotations;

namespace WorkFound.Application.Jobs.Dto.Application.Submit;

public record AppSubmitAnsweDto : IValidatableObject
{
    [Required(ErrorMessage = "Question is required")]
    public Guid QuestionId { get; set; }
    public string? AnswerText { get; set; }
    public List<Guid>? SelectedOptionIds { get; set; } = new();
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (QuestionId == Guid.Empty)
        {
            yield return new ValidationResult("QuestionId is required", [ nameof(QuestionId) ]);
        }
        
        if (string.IsNullOrWhiteSpace(AnswerText) && (SelectedOptionIds == null || !SelectedOptionIds.Any()))
        {
            yield return new ValidationResult("Either AnswerText or SelectedOptionIds must be provided", [ nameof(AnswerText), nameof(SelectedOptionIds) ]);
        }
        
        if (!string.IsNullOrWhiteSpace(AnswerText) && SelectedOptionIds != null && SelectedOptionIds.Any())
        {
            yield return new ValidationResult("Only one of AnswerText or SelectedOptionIds should be provided", [ nameof(AnswerText), nameof(SelectedOptionIds) ]);
        }
        
        if (SelectedOptionIds != null && SelectedOptionIds.Any() && SelectedOptionIds.Any(id => id == Guid.Empty))
        {
            yield return new ValidationResult("SelectedOptionIds cannot contain empty GUIDs", [ nameof(SelectedOptionIds) ]);
        }
        
    }
}