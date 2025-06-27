using WorkFound.Domain.Entities.Auth;

namespace WorkFound.Domain.Entities.Profile;

public class CompanyProfile
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public required AppUser AppUser { get; set; }
    
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? Website { get; set; }
    public string? LogoUrl { get; set; }
    public CompanyLocationType LocationType { get; set; }
    
    public string? Location { get; set; }
}