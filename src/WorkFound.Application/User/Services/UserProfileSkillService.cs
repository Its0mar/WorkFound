using Microsoft.EntityFrameworkCore;
using WorkFound.Application.Auth.Dtos.UserProfileDtos;
using WorkFound.Application.Common.Interface;
using WorkFound.Application.User.Interfaces;
using WorkFound.Domain.Entities.Common;

namespace WorkFound.Application.User.Services;

public class UserProfileSkillService : IUserProfileSkillService
{
    private readonly IAppDbContext _context;

    public UserProfileSkillService(IAppDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> AddSkill(UserSkillDto dto, Guid appUserId)
        {
            if (!GetUserProfileId(appUserId, out var userProfileId))
                return false;
            var userProfile = await _context.UserProfiles
                .Include(u => u.Skills)
                .FirstOrDefaultAsync(x => x.Id == userProfileId);
            if (userProfile is null) return false;
            
            if (userProfile.Skills.Any(s => s.Name.ToLower() == dto.SkillName.ToLower()))
                return false; // Skill already exists for this user
            var skill = await _context.Skills
                .FirstOrDefaultAsync(s => s.Name.ToLower() == dto.SkillName.ToLower());
            if (skill is null)
            {
                skill = new Skill() { Name = dto.SkillName };
                await _context.Skills.AddAsync(skill);
                await _context.SaveChangesAsync(); 
            }
    
            userProfile.Skills.Add(skill);
            return await _context.SaveChangesAsync() > 0 ;
        }
    public async Task<bool> RemoveSkill(Guid userId, Guid skillId)
        {
            if (!GetUserProfileId(userId, out var userProfileId))
                return false;
            
            var userProfile = await _context.UserProfiles
                .Include(up => up.Skills)
                .FirstOrDefaultAsync(up => up.Id == userProfileId);
            if (userProfile is null) return false;
            
            var skill = userProfile.Skills.FirstOrDefault(s => s.Id == skillId);
            if (skill is null) return false;
            userProfile.Skills.Remove(skill);
            
            bool isUsed = await _context.UserProfiles
                .AnyAsync(up => up.Id != userProfileId && up.Skills.Any(s => s.Id == skill.Id));
    
            bool isUsedByJob = await _context.Jobs
                .AnyAsync(jp => jp.Skills.Any(s => s.Id == skill.Id));
    
            if (!isUsed && !isUsedByJob)
            {
                _context.Skills.Remove(skill);
            }
            
            return await _context.SaveChangesAsync() > 0 ;
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