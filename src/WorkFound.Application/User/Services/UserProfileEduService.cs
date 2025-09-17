using Microsoft.EntityFrameworkCore;
using WorkFound.Application.Auth.Dto.UserProfileDtos;
using WorkFound.Application.Auth.Extensions;
using WorkFound.Application.Common.Interface;
using WorkFound.Application.User.Interfaces;

namespace WorkFound.Application.User.Services;

public class UserProfileEduService : IUserProfileEduService
{
    private readonly IAppDbContext _context;
    
    public UserProfileEduService(IAppDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> AddEducation(UserEducationDto dto, Guid appUserId)
    {
        if (!GetUserProfileId(appUserId, out var userProfileId))
            return false;
        
        var edu = dto.ToUserEducation(userProfileId);
        await _context.UserEducations.AddAsync(edu);
        return (await _context.SaveChangesAsync() > 0 ? true : false);
    }
    
    public async Task<bool> RemoveEducation(Guid id)
    {
        var edu = await _context.UserEducations.FirstOrDefaultAsync(e => e.Id == id);
        if (edu is null) return false;
        _context.UserEducations.Remove(edu);
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