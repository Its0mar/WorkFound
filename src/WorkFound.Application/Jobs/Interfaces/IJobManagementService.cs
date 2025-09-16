using WorkFound.Application.Jobs.Dto;

namespace WorkFound.Application.Jobs.Interfaces;

public interface IJobManagementService
{
    Task<bool> AddJobAsync(AddJobPostDto postDto, Guid appUserId);
    Task<bool> DeleteJobAsync(Guid jobId, Guid appUserId);
    Task<bool> OpenCloseJobAsync(Guid jobId, Guid appUserId);
    Task<bool> ShowHideJobAsync(Guid jobId, Guid appUserId);
    Task<bool> UpdateJobAsync(UpdateJobDto dto, Guid jobId, Guid appUserId);
}