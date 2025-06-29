namespace WorkFound.Application.Auth.Dtos;

public class UserRegisterDto : RegisterDto
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Location { get; init; }
    public string? Bio { get; init; }
    public string? ProfilePictureUrl { get; init; }
}