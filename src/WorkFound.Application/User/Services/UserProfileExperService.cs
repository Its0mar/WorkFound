using Microsoft.EntityFrameworkCore;
using WorkFound.Application.Auth.Dto.UserProfileDtos;
using WorkFound.Application.Auth.Extensions;
using WorkFound.Application.Common.Interface;
using WorkFound.Application.User.Interfaces;

namespace WorkFound.Application.User.Services;

public class UserProfileExperService : IUserProfileExperService
{
    private readonly IAppDbContext _context;
    public UserProfileExperService(IAppDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> AddExperince(UserExperinceDto dto, Guid appUserId)
        {
            if (!GetUserProfileId(appUserId, out var userProfileId))
                return false;
            
            var experience = dto.ToUserExperience(userProfileId);
            var userProfile = await _context.UserProfiles.Include(u => u.UserExperiences)
                .FirstOrDefaultAsync(u => u.Id == userProfileId);
            if (userProfile is null) return false;
            
            var duplicate = userProfile.UserExperiences.Any(exp => 
                exp.CompanyName.ToLower() == dto.CompanyName.ToLower() && 
                exp.Description?.ToLower() == dto.Description?.ToLower() && 
                exp.StartDate == dto.StartDate);
            
            if (duplicate) return false;

            userProfile.UserExperiences.Add(experience);
            return await _context.SaveChangesAsync() > 0;
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