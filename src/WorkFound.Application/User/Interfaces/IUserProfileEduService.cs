using WorkFound.Application.Auth.Dto.UserProfileDtos;

namespace WorkFound.Application.User.Interfaces;

public interface IUserProfileEduService
{
    public Task<bool> AddEducation(UserEducationDto dto, Guid userProfileId);
    public Task<bool> RemoveEducation(Guid id);
    
}