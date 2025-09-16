using Microsoft.EntityFrameworkCore;
using WorkFound.Application.ApplicationForm.Interfaces;
using WorkFound.Application.Common.Interface;
using WorkFound.Application.Jobs.Dto.Application.Apply;
using WorkFound.Domain.Entities.Enums;
using WorkFound.Domain.Entities.Jobs.Application.Forms;
using WorkFound.Domain.Entities.Jobs.Application.Questions;

namespace WorkFound.Application.ApplicationForm.Services;

public class JobApplicationFormService(IAppDbContext context) : IJobApplicationFormService
{
    public async Task<bool> CreateJobApplicationFormAsync(CreateJobApplicationFormDto dto, Guid appUserId)
    {
        if (!GetCompanyProfileId(appUserId, out var companyProfileId))
            return false;

        var job = await context.Jobs.Include(j => j.ApplicationForms).FirstOrDefaultAsync(j => j.Id == dto.JobId && j.CompanyId == companyProfileId);
        if (job is null) return false;

        var form = new JobApplicationForm()
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Version = job.ApplicationForms.Count + 1,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            JobId = job.Id,
            CoverLetterPrompt = dto.CoverLetterPrompt,
            ResumePrompt = dto.ResumePrompt,
            ThankYouMessage = dto.ThankYouMessage,
            Questions = dto.Questions.Select(qDto => new JobApplicationQuestion
            {
                Id = Guid.NewGuid(),
                Text = qDto.Text,
                Order = qDto.Order,
                IsRequired = qDto.IsRequired,
                QuestionType = (QuestionType)qDto.QuestionTypeDto,
                Options = qDto.Options?.Select((opt, index) => new JobApplicationQuestionOption
                {
                    Id = Guid.NewGuid(),
                    Text = opt,
                    Order = index + 1
                }).ToList() ?? null
            }).ToList()
        };

        await context.JobApplicationForms.AddAsync(form);
        job.ActiveFormId = form.Id;
        await context.JobApplicationForms.Where(f => f.JobId == job.Id && f.Id != form.Id && f.IsActive)
            .ExecuteUpdateAsync(setter => setter
                .SetProperty(f => f.IsActive, false)
                .SetProperty(f => f.DisabledAt, DateTime.UtcNow));

        return await context.SaveChangesAsync() > 0;
    }

    private bool GetCompanyProfileId(Guid appUserId, out Guid companyProfileId)
    {
        var company = context.CompanyProfiles.FirstOrDefault(c => c.AppUserId == appUserId);
        if (company is null)
        {
            companyProfileId = Guid.Empty;
            return false;
        }

        companyProfileId = company.Id;
        return true;
    }
}