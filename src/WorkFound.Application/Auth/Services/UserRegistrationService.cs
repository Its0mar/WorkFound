using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkFound.Application.Auth.Dto.Register;
using WorkFound.Application.Auth.Extensions;
using WorkFound.Application.Auth.Interfaces;
using WorkFound.Application.Common.Interface;
using WorkFound.Application.Common.Result;
using WorkFound.Domain.Entities.Auth;

namespace WorkFound.Application.Auth.Services;

public class UserRegistrationService : IUserRegistrationService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IAppDbContext _context;

    public UserRegistrationService(UserManager<AppUser> userManager, IAppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<AuthResult> CompanyRegisterAsync(CompanyRegisterDto dto)
    {
        var appUser = dto.ToAppUser(AccountType.Company);
        var result = await InitializeUser(appUser, dto.Password, appUser.AccountType.ToString());

        if (!result.Succeeded) return result;

        var companyProfile = dto.ToCompnyProfile(appUser);
        await _context.CompanyProfiles.AddAsync(companyProfile);
        await _context.SaveChangesAsync();

        return result;
    }

    public async Task<AuthResult> UserRegisterAsync(UserRegisterDto dto)
    {
        var appUser = dto.ToAppUser(AccountType.User);
        var result = await InitializeUser(appUser, dto.Password, appUser.AccountType.ToString());

        if (!result.Succeeded) return result;

        var userProfile = dto.ToUserProfile(appUser);
        await _context.UserProfiles.AddAsync(userProfile);
        await _context.SaveChangesAsync();

        return result;
    }

    private async Task<AuthResult> InitializeUser(AppUser appUser, string password, string role)
    {
        bool duplicatePhone = await _userManager.Users.AnyAsync(u => u.PhoneNumber == appUser.PhoneNumber);

        if (duplicatePhone)
            return AuthResult.Fail("Phone number already exists");

        appUser.RefreshToken = string.Empty; // To pass the non-null constraint
        var result = await _userManager.CreateAsync(appUser, password);
        if (!result.Succeeded)
            return AuthResult.Fail(result.Errors.Select(e => e.Description));

        result = await _userManager.AddToRoleAsync(appUser, role);
        if (!result.Succeeded)
            return AuthResult.Fail(result.Errors.Select(e => e.Description));

        return await CreateAuthResultAsync(appUser, role);
    }

    private async Task<AuthResult> CreateAuthResultAsync(AppUser appUser, string role)
    {
        var (refreshToken, expiryTime) = GenerateRefreshToken();
        appUser.RefreshToken = refreshToken;
        appUser.RefreshTokenExpiryTime = expiryTime;
        await _userManager.UpdateAsync(appUser);

        return AuthResult.Success(appUser.Id, token: null, refreshToken, expiryTime, role);
    }

    private (string token, DateTime ExpireOn) GenerateRefreshToken()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return (Convert.ToBase64String(randomBytes), DateTime.UtcNow.AddDays(7));
    }
}