using WorkFound.Domain.Entities.Profile.Company;

namespace WorkFound.Application.Auth.Dtos;

public class CompanyRegisterDto : RegisterDto
{
    public required string CompanyName { get; init; }
    public required string Description { get; init; }
    public string? Website { get; init; }
    public string? LogoUrl { get; init; }
    public required CompanyLocationType LocationType { get; init; }
    public string? Location { get; init; }
}