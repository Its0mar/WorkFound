using System.ComponentModel.DataAnnotations;

namespace WorkFound.Application.Jobs.Dto;

public record AddJobSkillDto
{
    [Required(ErrorMessage = "Skill is required")]
    [MaxLength(15, ErrorMessage = "Skill cannot exceed 30 characters")]
    [MinLength(2, ErrorMessage = "Skill must be at least 2 characters long")]
    public required List<string> Skills { get; set; } 
    
    public required Guid JobId { get; set; }
}