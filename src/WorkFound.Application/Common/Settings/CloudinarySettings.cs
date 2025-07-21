namespace WorkFound.Application.Common.Settings;

public record CloudinarySettings
{
    public required string CloudName { get; init; }
    public required string ApiKey { get; init; }
    public required string ApiSecret { get; init; }
}