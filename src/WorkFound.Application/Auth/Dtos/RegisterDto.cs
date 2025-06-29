using System.ComponentModel.DataAnnotations;
using WorkFound.Domain.Entities.Auth;

namespace WorkFound.Application.Auth.Dtos;

public class RegisterDto
{
    [Required(ErrorMessage = "Username is required")]
    public required string UserName { get; set; }

    [Required(ErrorMessage = $"{nameof(Email)} is required")] 
    public required string Email { get; set; }
    
    [Required(ErrorMessage = $"{nameof(Password)} is required")]
    public required string Password { get; set; }
    [Required(ErrorMessage = "Confirmation Password is required")]
    public required string ConfirmPassword { get; set; }
    
    [Required(ErrorMessage = "Phone Number is required")]
    public required string Phone { get; set; }
    [Required(ErrorMessage = "Account Type is required")]
    public required AccountType AccountType { get; set; }
    
    
}