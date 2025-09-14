using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WorkFound.Domain.Entities.Common;
using WorkFound.Domain.Entities.Jobs;
using WorkFound.Domain.Entities.Jobs.Application.Answers;
using WorkFound.Domain.Entities.Jobs.Application.Forms;
using WorkFound.Domain.Entities.Jobs.Application.Questions;
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
    DbSet<JobApplicationForm> JobApplicationForms { get; }
    DbSet<JobApplicationQuestion> JobApplicationQuestions { get; }
    DbSet<JobApplicationQuestionOption> JobApplicationQuestionOptions { get; }
    DbSet<JobAppSubmitForm> JobAppSubmitForms { get; }
    DbSet<JobAppSubmitAnswer> JobAppSubmitAnswers { get; }
    DbSet<JobAppSubmitAnswerOption> JobAppSubmitAnswerOptions { get; }
    DbSet<T> Set<T>() where T : class;
    Task<int> SaveChangesAsync();
}