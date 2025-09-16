using WorkFound.Application.Jobs.Dto;

namespace WorkFound.Application.Jobs.Interfaces;

public interface IJobSkillManagementService
{
    Task<bool> AddJobSkillAsync(AddJobSkillDto dto, Guid appUserId);
    Task<bool> RemoveJobSkillAsync(RemoveJobSkillDto dto, Guid appUserId);
}