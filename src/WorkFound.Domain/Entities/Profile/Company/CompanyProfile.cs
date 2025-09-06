using System.ComponentModel.DataAnnotations;
using WorkFound.Domain.Entities.Auth;
using WorkFound.Domain.Entities.Enums;
using WorkFound.Domain.Entities.Jobs;
using WorkFound.Domain.Entities.Profile.Company;

namespace WorkFound.Domain.Entities.Profile.Company;

public class CompanyProfile
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? Website { get; set; }
    public string? LogoUrl { get; set; }
    public CompanyLocationType LocationType { get; set; }
    public string? Location { get; set; }
    public List<JobPost> Jobs { get; set; } = new();
}
