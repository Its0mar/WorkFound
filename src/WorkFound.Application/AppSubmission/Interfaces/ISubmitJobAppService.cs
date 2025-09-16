using WorkFound.Application.Jobs.Dto.Application.Submit;

namespace WorkFound.Application.AppSubmission.Interfaces;

public interface ISubmitJobAppService
{
    public Task<bool> SubmitJobApplicationAsync(Guid userId, Guid jobId, AppSubmitDto dto);

}