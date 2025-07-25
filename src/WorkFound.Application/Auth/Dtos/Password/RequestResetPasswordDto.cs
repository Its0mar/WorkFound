using System.ComponentModel.DataAnnotations;

namespace WorkFound.Application.Auth.Dtos.Password;

public record RequestResetPasswordDto
{
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public required string Email { get; init; }
}