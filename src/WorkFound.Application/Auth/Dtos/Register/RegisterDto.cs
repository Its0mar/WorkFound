using System.ComponentModel.DataAnnotations;

namespace WorkFound.Application.Auth.Dtos.Register;

public record RegisterDto
{
    [Required(ErrorMessage = "Username is required")]
    public required string UserName { get; init; }

    [Required(ErrorMessage = $"{nameof(Email)} is required")] 
    [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email format")]
    public required string Email { get; init; }
    
    [Required(ErrorMessage = $"{nameof(Password)} is required")]
    [DataType(DataType.Password)]
    public required string Password { get; init; }
    [Required(ErrorMessage = "Confirmation Password is required")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    public required string ConfirmPassword { get; init; }
    
    [Required(ErrorMessage = "Phone Number is required")]
    [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid phone number format")]
    public required string Phone { get; init; }

    
}