using System.ComponentModel.DataAnnotations;

namespace WorkFound.Application.Jobs.Dto;

public record AddJobSkillDto : IValidatableObject
{
    [Required(ErrorMessage = "Skill is required")]
    [MinLength(1, ErrorMessage = "At least one skill must be provided")]
    public required List<string> Skills { get; set; } 
    
    public required Guid JobId { get; set; }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        //if (!Skills.Any())
          //  yield return new ValidationResult("At least one skill must be provided", [ nameof(Skills) ]);

        foreach (var skill in Skills)
        {
            if (string.IsNullOrWhiteSpace(skill) || skill.Length < 2 || skill.Length > 30)
                yield return new ValidationResult("Each skill must be between 2 and 30 characters long", [ nameof(Skills) ]);
        }

        
    }
}