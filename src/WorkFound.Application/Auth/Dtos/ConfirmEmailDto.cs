namespace WorkFound.Application.Auth.Dtos;

public record ConfirmEmailDto
{
    public required string Token { get; init; }
}