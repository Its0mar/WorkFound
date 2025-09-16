using System.ComponentModel.DataAnnotations;

namespace WorkFound.Application.Auth.Dto;

public record LoginDto
{
    [MinLength(5), MaxLength(30), RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$|^[a-zA-Z0-9._%+-]{5,30}$",
        ErrorMessage = "Email or Username must be a valid email address or a username with 5-30 alphanumeric characters.")]
    public required string EmailOrUsername { get; init; }
    [DataType(DataType.Password)]
    public required string Password { get; init; }
    public bool RememberMe { get; set; } = false;
}