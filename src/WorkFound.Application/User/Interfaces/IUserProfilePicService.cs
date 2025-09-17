namespace WorkFound.Application.User.Interfaces;

public interface IUserProfilePicService
{
    public Task<bool> UpdateProfilePicture(Guid appUserId, Stream fileStream, string fileName);
    public Task<bool> DeleteProfilePicture(Guid appUserId);
}