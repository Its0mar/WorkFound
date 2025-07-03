using System.ComponentModel.DataAnnotations;

namespace WorkFound.Application.Auth.Dtos;

public record ResetPasswordRequestDto
{
    [DataType(DataType.EmailAddress)]
    public required string  Email { get; init; }
}