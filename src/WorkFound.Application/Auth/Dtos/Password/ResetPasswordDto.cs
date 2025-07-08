using System.ComponentModel.DataAnnotations;

namespace WorkFound.Application.Auth.Dtos.Password;

public record ResetPasswordDto
{
    [DataType(DataType.Password)]
    public required string NewPassword { get; init; }
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match")]
    public required string ConfirmPassword { get; init; }
}