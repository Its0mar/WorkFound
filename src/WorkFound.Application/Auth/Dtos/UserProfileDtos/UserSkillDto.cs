using System.ComponentModel.DataAnnotations;

namespace WorkFound.Application.Auth.Dtos.UserProfileDtos;

public record UserSkillDto
{
    [Required(ErrorMessage = "Skill Name is required")]
    [MaxLength(15, ErrorMessage = "Skill Name cannot exceed 30 characters")]
    [MinLength(2, ErrorMessage = "Skill Name must be at least 2 characters long")]
    public required string SkillName { get; init; } 
}