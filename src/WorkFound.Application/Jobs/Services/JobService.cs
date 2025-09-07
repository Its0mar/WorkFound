using Microsoft.EntityFrameworkCore;
using WorkFound.Application.Auth.Extensions;
using WorkFound.Application.Common.Interface;
using WorkFound.Application.Jobs.Dto;
using WorkFound.Application.Validation;
using WorkFound.Domain.Entities.Common;

namespace WorkFound.Application.Jobs.Services;

public class JobService : IJobService
{
    private readonly IAppDbContext _context;

    public JobService(IAppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> AddJobAsync(AddJobPostDto postDto, Guid appUserId)
    {
        if (!GetCompanyProfileId(appUserId, out var companyProfileId))
            return false;
        
        var jobValidateResult = JobPostValidator.ValidateLocation(postDto.LocationType, postDto.Location);
        if (!jobValidateResult.IsValid) 
            return false;
        
        var job = postDto.ToJobPost(companyProfileId);
        await _context.Jobs.AddAsync(job);
        
        return (await _context.SaveChangesAsync() > 0 ? true : false);
    }

    public async Task<bool> DeleteJobAsync(Guid jobId, Guid appUserId)
    {
        if (!GetCompanyProfileId(appUserId, out var companyProfileId)) return false;
            
        var job = await _context.Jobs.FirstOrDefaultAsync
            (j => j.Id == jobId && j.CompanyId == companyProfileId);
       
        if (job is null) return false;
        
        _context.Jobs.Remove(job);
        return (await _context.SaveChangesAsync() > 0 ? true : false);
    }
    
    public async Task<bool> OpenCloseJobAsync(Guid jobId, Guid appUserId)
    {
        if (!GetCompanyProfileId(appUserId, out var companyProfileId)) return false;
        
        var job = await _context.Jobs.FirstOrDefaultAsync
            (j => j.Id == jobId && j.CompanyId == companyProfileId);
        
        if (job is null) return false;
        
        job.IsOpen = !job.IsOpen;
        _context.Jobs.Update(job);
        
        return (await _context.SaveChangesAsync() > 0 ? true : false);
    }

    public async Task<bool> ShowHideJobAsync(Guid jobId, Guid appUserId)
    {
        if (!GetCompanyProfileId(appUserId, out var companyProfileId)) return false;
        
        var job = await _context.Jobs.FirstOrDefaultAsync
            (j => j.Id == jobId && j.CompanyId == companyProfileId);
        
        if (job is null) return false;
        
        job.IsPublic = !job.IsPublic;
        _context.Jobs.Update(job);
        
        return (await _context.SaveChangesAsync() > 0 ? true : false);
    }

    public async Task<bool> UpdateJobAsync(UpdateJobDto dto, Guid jobId, Guid appUserId)
    {
        if (!GetCompanyProfileId(appUserId, out var companyProfileId)) return false;
        var job = await _context.Jobs.FirstOrDefaultAsync
            (j => j.Id == jobId && j.CompanyId == companyProfileId);
        
        if (job is null) return false;
        
        var jobValidateResult = JobPostValidator.ValidateLocation(dto.LocationType, dto.Location);
        if (!jobValidateResult.IsValid) 
            return false;
        
        job.Title = dto.Title;
        job.Description = dto.Description;
        job.Location = dto.Location;
        job.LocationType = dto.LocationType;
        
        _context.Jobs.Update(job);
        return (await _context.SaveChangesAsync() > 0 ? true : false);
    }

    public async Task<bool> AddJobSkillAsync(AddJobSkillDto dto, Guid appUserId)
    {
        if (!GetCompanyProfileId(appUserId, out var companyProfileId)) return false;
            
        var job = await _context.Jobs.Include(j => j.Skills).FirstOrDefaultAsync
            (j => j.Id == dto.JobId && j.CompanyId == companyProfileId);
        
        if (job is null) return false;

        foreach (var skillName in dto.Skills)
        {
            if (job.Skills.Any(s => s.Name.ToLower() == skillName.ToLower()))
                return false;
            var skill = await _context.Skills
                .FirstOrDefaultAsync(s => s.Name.ToLower() == skillName.ToLower());
            if (skill is null)
            {
                skill = new Skill() { Name = skillName };
                await _context.Skills.AddAsync(skill);
                await _context.SaveChangesAsync();
            }

            job.Skills.Add(skill);
        }

        return await _context.SaveChangesAsync() > 0;

    }

    public async Task<bool> RemoveJobSkillAsync(RemoveJobSkillDto dto, Guid appUserId)
    {
        if (!GetCompanyProfileId(appUserId, out var companyProfileId))
            return false;
        
        var job = await _context.Jobs.Include(j => j.Skills).FirstOrDefaultAsync
            (j => j.Id == dto.JobId && j.CompanyId == companyProfileId);
        
        if (job is null) return false;
        
        var skill = job.Skills.FirstOrDefault(s => s.Id == dto.SkillId);
        if (skill is null) return false;
        
        job.Skills.Remove(skill);
        
        bool isUsedByUser = await _context.UserProfiles.AnyAsync(u => u.Skills.Any(s => s.Id == dto.SkillId));
        bool isUsedByJobPost = await _context.Jobs.AnyAsync(u => u.Id != dto.JobId && u.Skills.Any(s => s.Id == dto.SkillId));

        if (!isUsedByUser && !isUsedByJobPost)
        {
            _context.Skills.Remove(skill);
        }
        
        return await _context.SaveChangesAsync() > 0;
    }
    
    public async Task<ViewJobPostDto?> GetPublicJobPostByIdAsync(Guid jobId)
    {
        var job = await _context.Jobs
            .Include(j => j.CompanyProfile)
            .Include(j => j.Skills)
            .FirstOrDefaultAsync(j => j.Id == jobId && j.IsPublic);
        if (job is null) return null;
        
        var jobDto = job.ToViewJobPostDto();
        return jobDto;
    }
    
    private bool GetCompanyProfileId(Guid appUserId, out Guid companyProfileId)
    {
        var company = _context.CompanyProfiles.FirstOrDefault(c => c.AppUserId == appUserId);
        if (company is null)
        {
            companyProfileId = Guid.Empty;
            return false;
        }
        
        companyProfileId = company.Id;
        return true;
    }
}