using WorkFound.Application.Auth.Dtos;
using WorkFound.Domain.Entities.Auth;
using WorkFound.Domain.Entities.Profile.Company;
using WorkFound.Domain.Entities.Profile.User;

namespace WorkFound.Application.Auth.Extensions;

public static class Mapping
{
    public static AppUser ToAppUser(this RegisterDto dto)
    {
        return new AppUser()
        {
            UserName = dto.UserName,
            Email = dto.Email,
            PhoneNumber = dto.Phone,
            CreatedAt = DateTime.Now,
            AccountType = AccountType.Company,
        };
    }

    public static CompanyProfile ToCompnyProfile(this CompanyRegisterDto dto, AppUser user)
    {
        return new CompanyProfile()
        {
            AppUser = user,
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
            AppUser = user,
            AppUserId = user.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Location = dto.Location,
            Bio = dto.Bio,
            ProfilePictureUrl = dto.ProfilePictureUrl,
        };
    }
}