using System.ComponentModel.DataAnnotations;
using WorkFound.Domain.Entities.Auth;

namespace WorkFound.Application.Auth.Dtos;

public class RegisterDto
{
    [Required(ErrorMessage = "Username is required")]
    public required string UserName { get; set; }

    [Required(ErrorMessage = $"{nameof(Email)} is required")] 
    [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email format")]
    public required string Email { get; set; }
    
    [Required(ErrorMessage = $"{nameof(Password)} is required")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
    [Required(ErrorMessage = "Confirmation Password is required")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    public required string ConfirmPassword { get; set; }
    
    [Required(ErrorMessage = "Phone Number is required")]
    [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid phone number format")]
    public required string Phone { get; set; }

    
}