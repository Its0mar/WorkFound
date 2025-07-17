using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkFound.Application.Common.Interface;
using WorkFound.Domain.Entities.Auth;
using WorkFound.Domain.Entities.Jobs;
using WorkFound.Domain.Entities.Profile.Admin;
using WorkFound.Domain.Entities.Profile.Company;
using WorkFound.Domain.Entities.Profile.User;

namespace WorkFound.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>(options), IAppDbContext
{
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<UserSkill> UserSkills => Set<UserSkill>();
    public DbSet<UserExperience> UserExperiences => Set<UserExperience>();
    public DbSet<UserEducation> UserEducations => Set<UserEducation>();
    public DbSet<CompanyProfile> CompanyProfiles => Set<CompanyProfile>();
    public DbSet<Job> Jobs => Set<Job>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>()
            .HasOne(x => x.UserProfile)
            .WithOne(x => x.AppUser)
            .HasForeignKey<UserProfile>(x => x.AppUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<AppUser>()
            .HasOne(x => x.CompanyProfile)
            .WithOne(x => x.AppUser)
            .HasForeignKey<CompanyProfile>(x => x.AppUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<AppUser>()
            .HasOne(x => x.AdminProfile)
            .WithOne(x => x.AppUser)
            .HasForeignKey<AdminProfile>(x => x.AppUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Ensure unique constraints for phone number
        builder.Entity<AppUser>()
            .HasIndex(u => u.PhoneNumber)
            .IsUnique();
        
        builder.Entity<UserProfile>()
            .HasMany(up => up.UserSkills)
            .WithOne(us => us.UserProfile)
            .HasForeignKey(us => us.UserProfileId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<UserProfile>()
            .HasMany(up => up.UserExperiences)
            .WithOne(ue => ue.UserProfile)
            .HasForeignKey(ue => ue.UserProfileId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<UserProfile>()
            .HasMany(up => up.UserEducations)
            .WithOne(ue => ue.UserProfile)
            .HasForeignKey(ue => ue.UserProfileId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<CompanyProfile>()
            .HasMany(cp => cp.Jobs)
            .WithOne(j => j.CompanyProfile)
            .HasForeignKey(j => j.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);
        
        //to remove the length warning
        // builder.Entity<AppUser>(entiy =>
        // {
        //     entiy.Property(p => p.RefreshToken)
        //         .HasMaxLength(50);
        // });
        
        // to remove the length warning
        builder.Entity<UserProfile>(entity =>
        {
            entity.Property(p => p.FirstName)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(p => p.LastName)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(p => p.Location)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(p => p.Bio)
                .HasMaxLength(500);
            
            entity.Property(p => p.ProfilePictureUrl)
                .HasMaxLength(255);
        });
        
        builder.Entity<IdentityRole<Guid>>().HasData(
            new IdentityRole<Guid>
            {
                Id = Guid.Parse("b0a3d7f1-f403-4f5e-9457-f7f8bcb963a9"), // Fixed ID for referential integrity
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole<Guid>
            {
                Id = Guid.Parse("cfa01de5-1f98-4907-a8a5-9b42a6bb61dd"),
                Name = "Company",
                NormalizedName = "COMPANY"
            },
            new IdentityRole<Guid>
            {
                Id = Guid.Parse("46de1a26-355f-4901-b7c4-7c122f6d1fa7"),
                Name = "User",
                NormalizedName = "USER"
            }
        );
    }
    
    
    public Task<int> SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }

}