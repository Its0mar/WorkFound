using WorkFound.Application.Auth.Dtos.UserProfileDtos;

namespace WorkFound.Application.User.Services;

public interface IUserService
{

    public Task<bool> UpdateProfilePicture(Guid appUserId, Stream fileStream, string fileName);
    public Task<bool> DeleteProfilePicture(Guid appUserId);
    public Task<bool> AddEducation(UserEducationDto dto, Guid userProfileId);
    public Task<bool> AddSkill(UserSkillDto dto, Guid userProfileId);
    public Task<bool> AddExperince(UserExperinceDto dto, Guid appUserId);

    public Task<bool> RemoveEducation(Guid id);
    public Task<bool> RemoveSkill(Guid userId, Guid skillId);
    public Task<bool> RemoveExperince(Guid id);


}