using Microsoft.EntityFrameworkCore;
using WorkFound.Application.Auth.Extensions;
using WorkFound.Application.Common.Interface;
using WorkFound.Application.Jobs.Dto;
using WorkFound.Application.Jobs.Interfaces;
using WorkFound.Application.Validation;

namespace WorkFound.Application.Jobs.Services;

public class JobManagementService(IAppDbContext context) : IJobManagementService
{
    public async Task<bool> AddJobAsync(AddJobPostDto postDto, Guid appUserId)
    {
        if (!GetCompanyProfileId(appUserId, out var companyProfileId))
            return false;

        var jobValidateResult = JobPostValidator.ValidateLocation(postDto.LocationType, postDto.Location);
        if (!jobValidateResult.IsValid)
            return false;

        var job = postDto.ToJobPost(companyProfileId);
        await context.Jobs.AddAsync(job);

        return await context.SaveChangesAsync() > 0;
    }
    
    public async Task<bool> UpdateJobAsync(UpdateJobDto dto, Guid jobId, Guid appUserId)
    {
        if (!GetCompanyProfileId(appUserId, out var companyProfileId)) return false;
        var job = await context.Jobs.FirstOrDefaultAsync(j => j.Id == jobId && j.CompanyId == companyProfileId);

        if (job is null) return false;

        var jobValidateResult = JobPostValidator.ValidateLocation(dto.LocationType, dto.Location);
        if (!jobValidateResult.IsValid)
            return false;

        job.Title = dto.Title;
        job.Description = dto.Description;
        job.Location = dto.Location;
        job.LocationType = dto.LocationType;

        context.Jobs.Update(job);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteJobAsync(Guid jobId, Guid appUserId)
    {
        if (!GetCompanyProfileId(appUserId, out var companyProfileId)) return false;

        var job = await context.Jobs.FirstOrDefaultAsync(j => j.Id == jobId && j.CompanyId == companyProfileId);
        if (job is null) return false;

        context.Jobs.Remove(job);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> OpenCloseJobAsync(Guid jobId, Guid appUserId)
    {
        if (!GetCompanyProfileId(appUserId, out var companyProfileId)) return false;

        var job = await context.Jobs.FirstOrDefaultAsync(j => j.Id == jobId && j.CompanyId == companyProfileId);
        if (job is null) return false;

        job.IsOpen = !job.IsOpen;
        context.Jobs.Update(job);

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> ShowHideJobAsync(Guid jobId, Guid appUserId)
    {
        if (!GetCompanyProfileId(appUserId, out var companyProfileId)) return false;

        var job = await context.Jobs.FirstOrDefaultAsync(j => j.Id == jobId && j.CompanyId == companyProfileId);
        if (job is null) return false;

        job.IsPublic = !job.IsPublic;
        context.Jobs.Update(job);

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