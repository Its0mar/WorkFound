using Microsoft.EntityFrameworkCore;
using WorkFound.Application.Auth.Dtos.UserProfileDtos;
using WorkFound.Application.Auth.Extensions;
using WorkFound.Application.Common.Interface;
using WorkFound.Application.Files;

namespace WorkFound.Application.User.Services;

public class UserService : IUserService
{
    private readonly IAppDbContext _context;
    private readonly IFileService _fileService;

    public UserService(IAppDbContext context, IFileService fileService)
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
    public async Task<bool> AddEducation(UserEducationDto dto, Guid appUserId)
    {
        if (!GetUserProfileId(appUserId, out var userProfileId))
            return false;
        
        var edu = dto.ToUserEducation(userProfileId);
        await _context.UserEducations.AddAsync(edu);
        return (await _context.SaveChangesAsync() > 0 ? true : false);
    }
    
    public async Task<bool> AddSkill(UserSkillDto dto, Guid appUserId)
    {
        if (!GetUserProfileId(appUserId, out var userProfileId))
            return false;
        
        var skill = dto.ToUserSkill(userProfileId);
        await _context.UserSkills.AddAsync(skill);
        return (await _context.SaveChangesAsync() > 0 ? true : false);
    }
    
    public async Task<bool> AddExperince(UserExperinceDto dto, Guid appUserId)
    {
        if (!GetUserProfileId(appUserId, out var userProfileId))
            return false;
        
        var experience = dto.ToUserExperience(userProfileId);
        await _context.UserExperiences.AddAsync(experience);
        return (await _context.SaveChangesAsync() > 0 ? true : false);
    }
    
    public async Task<bool> RemoveEducation(Guid id)
    {
        var edu = await _context.UserEducations.FirstOrDefaultAsync(e => e.Id == id);
        if (edu is null) return false;
        _context.UserEducations.Remove(edu);
        return (await _context.SaveChangesAsync() > 0 ? true : false);
    }
    
    public async Task<bool> RemoveSkill(Guid id)
    {
        var skill = await _context.UserSkills.FirstOrDefaultAsync(s => s.Id == id);
        if (skill is null) return false;
        _context.UserSkills.Remove(skill);
        return (await _context.SaveChangesAsync() > 0 ? true : false);
    }
    
    public async Task<bool> RemoveExperince(Guid id)
    {
        var exp = await _context.UserExperiences.FirstOrDefaultAsync(e => e.Id == id);
        if (exp is null) return false;
        _context.UserExperiences.Remove(exp);
        return (await _context.SaveChangesAsync() > 0 ? true : false);
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