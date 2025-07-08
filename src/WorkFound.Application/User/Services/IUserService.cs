using WorkFound.Application.Auth.Dtos.UserProfileDtos;

namespace WorkFound.Application.User.Services;

public interface IUserService
{
    public Task<bool> AddEducation(UserEducationDto dto, Guid userProfileId);
    public Task<bool> AddSkill(UserSkillDto dto, Guid userProfileId);
    public Task<bool> AddExperince(UserExperinceDto dto, Guid appUserId);

    public Task<bool> RemoveEducation(Guid id);
    public Task<bool> RemoveSkill(Guid id);
    public Task<bool> RemoveExperince(Guid id);


}