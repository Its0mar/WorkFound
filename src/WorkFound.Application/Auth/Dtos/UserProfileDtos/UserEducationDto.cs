using System.ComponentModel.DataAnnotations;
using WorkFound.Application.Auth.ValidationsAttribute;

namespace WorkFound.Application.Auth.Dtos.UserProfileDtos;

public record UserEducationDto
{
    [Required(ErrorMessage = "School Name is required")]
    [MaxLength(15, ErrorMessage = "School Name cannot exceed 30 characters")]
    [MinLength(2, ErrorMessage = "School Name must be at least 2 characters long")]
    public required string SchoolName { get; init; }
    [Required(ErrorMessage = "Degree is required")]
    [MaxLength(15, ErrorMessage = "Degree cannot exceed 30 characters")]
    [MinLength(2, ErrorMessage = "Degree must be at least 2 characters long")]
    public required string Degree { get; init; }
    [Required(ErrorMessage = "Field of Study is required")]
    [MaxLength(15, ErrorMessage = "Field of Study cannot exceed 30 characters")]
    [MinLength(2, ErrorMessage = "Field of Study must be at least 2 characters long")]
    public required string FieldOfStudy { get; init; }
    [Required(ErrorMessage = "Start Date is required")]
    [NotFutureDate]
    public required DateTime StartDate { get; init; }
    [NotFutureDate]
    [EndDateAfterStartDate(nameof(StartDate))]
    public DateTime? EndDate { get; init; }
}
