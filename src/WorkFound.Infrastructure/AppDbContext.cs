using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkFound.Domain.Entities.Auth;
using WorkFound.Domain.Entities.Profile;

namespace WorkFound.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<CompanyProfile> CompanyProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<AppUser>()
            .HasOne(x => x.UserProfile)
            .WithOne(x => x.AppUser)
            .HasForeignKey<UserProfile>(x => x.AppUserId);
        
        builder.Entity<AppUser>()
            .HasOne(x => x.CompanyProfile)
            .WithOne(x => x.AppUser)
            .HasForeignKey<CompanyProfile>(x => x.AppUserId);;
    }
}