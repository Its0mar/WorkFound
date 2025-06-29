using WorkFound.Domain.Entities.Auth;

namespace WorkFound.Domain.Entities.Profile.Admin;

public class AdminProfile
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
    
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}