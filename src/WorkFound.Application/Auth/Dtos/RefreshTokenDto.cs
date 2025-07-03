namespace WorkFound.Application.Auth.Dtos;

public record RefreshTokenDto
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}