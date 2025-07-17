using WorkFound.Application.Jobs.Dto;

namespace WorkFound.Application.Jobs.Services;

public interface IJobService
{
    public Task<bool> AddJobAsync(AddJobDto dto, Guid appUserId);
    public Task<bool> DeleteJobAsync(Guid jobId, Guid appUserId);
    public Task<bool> OpenCloseJobAsync(Guid jobId, Guid appUserId);
    public Task<bool> ShowHideJobAsync(Guid jobId, Guid appUserId);
    public Task<bool> UpdateJobAsync(UpdateJobDto dto, Guid jobId, Guid appUserId);


}