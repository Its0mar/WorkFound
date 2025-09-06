using Microsoft.EntityFrameworkCore;
using WorkFound.Domain.Entities.Common;
using WorkFound.Domain.Entities.Jobs;
using WorkFound.Domain.Entities.Profile.Company;
using WorkFound.Domain.Entities.Profile.User;

namespace WorkFound.Application.Common.Interface;

public interface IAppDbContext
{
    DbSet<CompanyProfile> CompanyProfiles { get; }
    DbSet<UserProfile> UserProfiles { get; }
    DbSet<UserExperience> UserExperiences { get; }
    DbSet<UserEducation> UserEducations { get; }
    DbSet<Skill> Skills { get; }
    DbSet<JobPost> Jobs { get; }
    DbSet<T> Set<T>() where T : class;
    Task<int> SaveChangesAsync();
}