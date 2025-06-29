using Microsoft.AspNetCore.Identity;
using WorkFound.Domain.Entities.Profile.Admin;
using WorkFound.Domain.Entities.Profile.Company;
using WorkFound.Domain.Entities.Profile.User;

namespace WorkFound.Domain.Entities.Auth;

public class AppUser : IdentityUser<Guid>
{
    public DateTime CreatedAt { get; set; }
    public AccountType AccountType { get; set; }
    public UserProfile? UserProfile { get; set; }
    public CompanyProfile? CompanyProfile { get; set; }
    public AdminProfile? AdminProfile { get; set; }
    
}


