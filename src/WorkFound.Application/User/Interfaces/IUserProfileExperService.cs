using WorkFound.Application.Auth.Dto.UserProfileDtos;

namespace WorkFound.Application.User.Interfaces;

public interface IUserProfileExperService
{
    public Task<bool> AddExperince(UserExperinceDto dto, Guid appUserId);
    public Task<bool> RemoveExperince(Guid id);
    
}