using System.ComponentModel.DataAnnotations;
using WorkFound.Application.Auth.ValidationsAttribute;

namespace WorkFound.Application.Auth.Dtos.UserProfileDtos;

public record UserExperinceDto
{
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(15, ErrorMessage = "Title cannot exceed 30 characters")]
    [MinLength(2, ErrorMessage = "Title must be at least 2 characters long")]
    public required string Title { get; init; }

    [MaxLength(15, ErrorMessage = "Description cannot exceed 200 characters")]
    [MinLength(2, ErrorMessage = "Description must be at least 15 characters long")]
    public string? Description { get; init; }
    [Required(ErrorMessage = "Start Date is required")]
    [NotFutureDate]
    public DateTime StartDate { get; init; }
    [NotFutureDate]
    [EndDateAfterStartDate(nameof(StartDate))]
    public DateTime? EndDate { get; init; }
    [Required(ErrorMessage = "Company Name is required")]
    [MaxLength(15, ErrorMessage = "Company Name cannot exceed 30 characters")]
    [MinLength(2, ErrorMessage = "Company Name must be at least 3 characters long")]
    public required string CompanyName { get; init; }
}

//TODO : add location , duaration in dto or here if needed