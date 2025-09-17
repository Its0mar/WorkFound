using Microsoft.EntityFrameworkCore;
using WorkFound.Application.Common.Interface;
using WorkFound.Application.Files;
using WorkFound.Application.User.Interfaces;

namespace WorkFound.Application.User.Services;

public class UserProfilePicService : IUserProfilePicService
{
    private readonly IAppDbContext _context;
    private readonly IFileService _fileService;

    public UserProfilePicService(IAppDbContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }
    
    public async Task<bool> UpdateProfilePicture(Guid appUserId, Stream fileStream, string fileName)
    {
        if (!GetUserProfileId(appUserId, out var userProfileId))
            return false;
        
        var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.Id == userProfileId);
        if (userProfile is null)
            return false;
        
        var result = await _fileService.UploadFileAsync(fileStream, fileName, "profile_pictures", true);
        if (string.IsNullOrEmpty(result))
            return false;
        
        if (userProfile.ProfilePictureUrl != null)
        {
            var deleteResult = await _fileService.DeleteFileAsync(userProfile.ProfilePictureUrl);
            if (!deleteResult)
                return false;
        }
        userProfile.ProfilePictureUrl = result;
        _context.UserProfiles.Update(userProfile);

        return await _context.SaveChangesAsync() > 0;
    }
    
    public async Task<bool> DeleteProfilePicture(Guid appUserId)
    {
        if (!GetUserProfileId(appUserId, out var userProfileId)) return false;
        var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.Id == userProfileId);
        if (userProfile is null || string.IsNullOrEmpty(userProfile.ProfilePictureUrl))
            return false;
        
        var deleteResult = await _fileService.DeleteFileAsync(userProfile.ProfilePictureUrl);
        if (!deleteResult)
            return false;
        
        userProfile.ProfilePictureUrl = null;
        _context.UserProfiles.Update(userProfile);
        
        return await _context.SaveChangesAsync() > 0;
    }
    
    
    
    private bool GetUserProfileId(Guid appUserId, out Guid userProfileId)
    {
        var userProfile = _context.UserProfiles.FirstOrDefault(x => x.AppUserId == appUserId);
        if (userProfile is null)
        {
            userProfileId = Guid.Empty;
            return false;
        }
        
        userProfileId = userProfile.Id;
        return true;
    }
}