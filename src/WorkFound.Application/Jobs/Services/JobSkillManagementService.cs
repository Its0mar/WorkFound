using Microsoft.EntityFrameworkCore;
using WorkFound.Application.Common.Interface;
using WorkFound.Application.Jobs.Dto;
using WorkFound.Application.Jobs.Interfaces;
using WorkFound.Domain.Entities.Common;

namespace WorkFound.Application.Jobs.Services;

public class JobSkillManagementService(IAppDbContext context) : IJobSkillManagementService
{
    public async Task<bool> AddJobSkillAsync(AddJobSkillDto dto, Guid appUserId)
    {
        if (!GetCompanyProfileId(appUserId, out var companyProfileId)) return false;

        var job = await context.Jobs.Include(j => j.Skills).FirstOrDefaultAsync(j => j.Id == dto.JobId && j.CompanyId == companyProfileId);
        if (job is null) return false;

        foreach (var skillName in dto.Skills)
        {
            if (job.Skills.Any(s => s.Name.ToLower() == skillName.ToLower()))
                return false;
            var skill = await context.Skills.FirstOrDefaultAsync(s => s.Name.ToLower() == skillName.ToLower());
            if (skill is null)
            {
                skill = new Skill { Name = skillName };
                await context.Skills.AddAsync(skill);
                await context.SaveChangesAsync();
            }

            job.Skills.Add(skill);
        }

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveJobSkillAsync(RemoveJobSkillDto dto, Guid appUserId)
    {
        if (!GetCompanyProfileId(appUserId, out var companyProfileId))
            return false;

        var job = await context.Jobs.Include(j => j.Skills).FirstOrDefaultAsync(j => j.Id == dto.JobId && j.CompanyId == companyProfileId);
        if (job is null) return false;

        var skill = job.Skills.FirstOrDefault(s => s.Id == dto.SkillId);
        if (skill is null) return false;

        job.Skills.Remove(skill);

        bool isUsedByUser = await context.UserProfiles.AnyAsync(u => u.Skills.Any(s => s.Id == dto.SkillId));
        bool isUsedByJobPost = await context.Jobs.AnyAsync(u => u.Id != dto.JobId && u.Skills.Any(s => s.Id == dto.SkillId));

        if (!isUsedByUser && !isUsedByJobPost)
        {
            context.Skills.Remove(skill);
        }

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