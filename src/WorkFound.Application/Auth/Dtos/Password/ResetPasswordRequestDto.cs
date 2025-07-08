using System.ComponentModel.DataAnnotations;

namespace WorkFound.Application.Auth.Dtos.Password;

public record ResetPasswordRequestDto
{
    [DataType(DataType.EmailAddress)]
    public required string  Email { get; init; }
}