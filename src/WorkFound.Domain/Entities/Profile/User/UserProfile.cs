using System.ComponentModel.DataAnnotations;
using WorkFound.Domain.Entities.Auth;

namespace WorkFound.Domain.Entities.Profile.User;

public class UserProfile
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Location { get; set; }
    public string? Bio { get; set; }
    public string? ProfilePictureUrl { get; set; }
    
    public string Name => $"{FirstName} {LastName}";
}