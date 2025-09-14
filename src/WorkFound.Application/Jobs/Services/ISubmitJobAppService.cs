using WorkFound.Application.Jobs.Dto.Application.Submit;

namespace WorkFound.Application.Jobs.Services;

public interface ISubmitJobAppService
{
    public Task<bool> SubmitJobApplicationAsync(Guid userId, Guid jobId, AppSubmitDto dto);

}