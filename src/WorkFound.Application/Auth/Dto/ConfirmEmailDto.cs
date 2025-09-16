namespace WorkFound.Application.Auth.Dto;

public record ConfirmEmailDto
{
    public required string Token { get; init; }
}