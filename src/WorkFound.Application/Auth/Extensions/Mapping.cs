using WorkFound.Application.Auth.Dtos.Register;
using WorkFound.Application.Auth.Dtos.UserProfileDtos;
using WorkFound.Domain.Entities.Auth;
using WorkFound.Domain.Entities.Profile.User;
using WorkFound.Domain.Entities.Profile.Company;


namespace WorkFound.Application.Auth.Extensions;

public static class Mapping
{
    public static AppUser ToAppUser(this RegisterDto dto, AccountType accTyoe)
    {
        return new AppUser()
        {
            UserName = dto.UserName,
            Email = dto.Email,
            PhoneNumber = dto.Phone,
            CreatedAt = DateTime.Now,
            AccountType = accTyoe,
        };
    }

    public static CompanyProfile ToCompnyProfile(this CompanyRegisterDto dto, AppUser user)
    {
        return new CompanyProfile()
        {
            // AppUser = user,
            AppUserId = user.Id,
            Name = dto.CompanyName,
            Description = dto.Description,
            Website = dto.Website,
            LogoUrl = dto.LogoUrl,
            Location = dto.Location,
            LocationType = dto.LocationType,
        };
    }
    
    public static UserProfile ToUserProfile(this UserRegisterDto dto, AppUser user)
    {
        return new UserProfile()
        {
            // AppUser = user,
            AppUserId = user.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Location = dto.Location,
            Bio = dto.Bio,
            ProfilePictureUrl = dto.ProfilePictureUrl,
        };
    }
    
    public static UserExperience ToUserExperience(this UserExperinceDto dto, Guid userProfileId)
    {
        return new UserExperience()
        {
            UserProfileId = userProfileId,
            CompanyName = dto.CompanyName,
            Title = dto.Title,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Description = dto.Description,
        };
    }

    public static UserSkill ToUserSkill(this UserSkillDto dto, Guid userProfileId)
    {
        return new UserSkill()
        {
            SkillName = dto.SkillName,
            UserProfileId = userProfileId,
        };
    }
    
    public static UserEducation ToUserEducation(this UserEducationDto dto, Guid userProfileId)
    {
        return new UserEducation()
        {
            UserProfileId = userProfileId,
            Degree = dto.Degree,
            FieldOfStudy = dto.FieldOfStudy,
            SchoolName = dto.SchoolName,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
        };
    }
        
    
}