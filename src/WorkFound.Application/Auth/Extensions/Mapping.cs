using WorkFound.Application.Auth.Dtos.Register;
using WorkFound.Application.Auth.Dtos.UserProfileDtos;
using WorkFound.Application.Jobs.Dto;
using WorkFound.Domain.Entities.Auth;
using WorkFound.Domain.Entities.Common;
using WorkFound.Domain.Entities.Jobs;
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

    public static Skill ToUserSkill(this UserSkillDto dto, Guid userProfileId)
    {
        return new Skill()
        {
            Name = dto.SkillName,
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
        
    public static JobPost ToJobPost(this AddJobPostDto postDto, Guid companyId)
    {
        return new JobPost()
        {
            Title = postDto.Title,
            Description = postDto.Description,
            CompanyId = companyId,
            Location = postDto.Location,
            LocationType = postDto.LocationType,
            IsOpen = postDto.IsOpen,
            IsPublic = postDto.IsPublic,
        };
    }

    public static ViewJobPostDto ToViewJobPostDto(this JobPost jobPost)
    {
        return new ViewJobPostDto()
        {
            Id = jobPost.Id,
            Title = jobPost.Title,
            Description = jobPost.Description,
            LocationType = jobPost.LocationType.ToString(),
            Location = jobPost.Location,
            IsOpen = jobPost.IsOpen,
            IsPublic = jobPost.IsPublic,
            CreatedAt = jobPost.CreatedAt,
            CompanyName = jobPost.CompanyProfile?.Name ?? string.Empty,
            CompanyLogoUrl = jobPost.CompanyProfile?.LogoUrl,
            Skills = jobPost.Skills.Select(s => s.Name).ToList(),
        };
    }
}