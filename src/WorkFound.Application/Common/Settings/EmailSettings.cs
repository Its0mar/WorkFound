namespace WorkFound.Application.Common.Settings;

public record EmailSettings
{
    public required string Host { get; init; }
    public required string Port { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string From { get; init; }
    public required string DisplayName { get; init; }
    public required string EnableSsl { get; init; }
}