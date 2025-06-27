using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WorkFound.Domain.Entities.Profile;

namespace WorkFound.Domain.Entities.Auth;

public class AppUser : IdentityUser<Guid>
{
    public DateTime CreatedAt { get; set; }
    public AccountType AccountType { get; set; }
    
    public UserProfile? UserProfile { get; set; }
    public CompanyProfile? CompanyProfile { get; set; }
    
}

