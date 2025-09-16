using WorkFound.Application.Jobs.Dto.Application.Apply;

namespace WorkFound.Application.ApplicationForm.Interfaces;

public interface IJobApplicationFormService
{
    Task<bool> CreateJobApplicationFormAsync(CreateJobApplicationFormDto dto, Guid appUserId);
}