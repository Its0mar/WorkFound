using System.ComponentModel.DataAnnotations;

namespace WorkFound.Application.Auth.Dto.Register;

public record UserRegisterDto : RegisterDto
{
    [Required(ErrorMessage = "First Name is required")]
    [MaxLength(15, ErrorMessage = "First Name cannot exceed 15 characters")]
    [MinLength(2, ErrorMessage = "First Name must be at least 2 characters long")]
    public required string FirstName { get; init; }
    [Required(ErrorMessage = "Last Name is required")]
    [MaxLength(15, ErrorMessage = "Last Name cannot exceed 15 characters")]
    [MinLength(2, ErrorMessage = "Last Name must be at least 2 characters long")]
    public required string LastName { get; init; }
    [Required(ErrorMessage = "Location is required")]
    public required string Location { get; init; }
    public string? Bio { get; init; }
    public string? ProfilePictureUrl { get; init; }
}