using WorkFound.Application.Jobs.Dto;
using WorkFound.Application.Jobs.Dto.Application.Apply;

namespace WorkFound.Application.Jobs.Services;

public interface IJobService
{
    public Task<bool> AddJobAsync(AddJobPostDto postDto, Guid appUserId);
    public Task<bool> DeleteJobAsync(Guid jobId, Guid appUserId);
    public Task<bool> OpenCloseJobAsync(Guid jobId, Guid appUserId);
    public Task<bool> ShowHideJobAsync(Guid jobId, Guid appUserId);
    public Task<bool> UpdateJobAsync(UpdateJobDto dto, Guid jobId, Guid appUserId);
    public Task<bool> AddJobSkillAsync(AddJobSkillDto dto, Guid appUserId);
    public Task<bool> RemoveJobSkillAsync(RemoveJobSkillDto dto, Guid appUserId);
    public Task<ViewJobPostDto?> GetPublicJobPostByIdAsync(Guid jobId);
    public Task<bool> CreateJobApplicationFormAsync(CreateJobApplicationFormDto dto, Guid appUserId);



}