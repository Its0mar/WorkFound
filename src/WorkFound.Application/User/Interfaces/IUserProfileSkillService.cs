using WorkFound.Application.Auth.Dtos.UserProfileDtos;

namespace WorkFound.Application.User.Interfaces;

public interface IUserProfileSkillService
{
    public Task<bool> AddSkill(UserSkillDto dto, Guid userProfileId);
    public Task<bool> RemoveSkill(Guid userId, Guid skillId);
    
    
}