using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkFound.Application.Auth.Dtos;
using WorkFound.Application.Auth.Extensions;
using WorkFound.Application.Auth.TokenGenerator;
using WorkFound.Application.Common.Interface;
using WorkFound.Application.Common.Result;
using WorkFound.Domain.Entities.Auth;

namespace WorkFound.Application.Auth.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IAppDbContext _context;
    private readonly IMailService _mailService;

    public AuthService(UserManager<AppUser> userManager, IJwtTokenGenerator jwtTokenGenerator,
        IAppDbContext context, IMailService mailService)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _context = context;
        _mailService = mailService;
    }
    
    #region Registration and Login Methods
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
    
    public async Task<AuthResult> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.EmailOrUsername) ??
                   await _userManager.FindByNameAsync(dto.EmailOrUsername) ??
                   await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == dto.EmailOrUsername);
        
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            return AuthResult.Fail("Invalid credentials");
        
        return await CreateAuthResultAsync(user, user.AccountType.ToString());
    }
    
    #endregion

    #region Token Methods

    public async Task<AuthResult> RefreshTokenAsync(string token)
    {
        var user = await  _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == token);
        
        if (user is null || !user.IsTokenActive)
            return AuthResult.Fail("Invalid token");
        
        return await CreateAuthResultAsync(user, user.AccountType.ToString());
    }
    
    public async Task<bool> RevokeTokenAsync(string token)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == token);
        if (user is null|| user.IsTokenActive) return false;
        
        user.IsTokenRevoked = true;
        
        return (await _userManager.UpdateAsync(user)).Succeeded;
    }

    #endregion

    #region Password Management Methods

    public async Task<AuthResult> ChangePasswordAsync(ChangePasswordDto dto, Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        
        if (user is null) return AuthResult.Fail("User not found");
        
        var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
        if (!result.Succeeded)
            return AuthResult.Fail(result.Errors.Select(e => e.Description));

        await RevokeTokenAsync(user.RefreshToken);
        return AuthResult.Success(userId, token: _jwtTokenGenerator.GenerateToken(user),user.AccountType.ToString());
        
    }
    
    public async Task<AuthResult> ResetPasswordAsync(ResetPasswordDto dto, string email, string token)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return AuthResult.Fail("User not found");

        var result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);
        if (!result.Succeeded)
            return AuthResult.Fail(result.Errors.Select(e => e.Description));
        
        await RevokeTokenAsync(user.RefreshToken);
        return await CreateAuthResultAsync(user, user.AccountType.ToString());
    }

    #endregion
    
    public async Task<AuthResult> ConfirmEmailAsync(string token, Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        
        if (user is null) return AuthResult.Fail("User not found");
        
        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
            return AuthResult.Fail(result.Errors.Select(e => e.Description));

        return AuthResult.Success(userId,role: user.AccountType.ToString());
    }

    public async Task SendResetPasswordEmailAsync(string email, string resetLink)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return;
        
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetUrl = $"{resetLink}?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(email)}";
        var subject = "Reset your password";
        var body = $"Please reset your password by clicking this link: <a href='{resetUrl}'>Reset Password</a>";
        
        await _mailService.SendEmailAsync(user.Email!, subject, body);

    }

    
    #region Utility Methods
    // a method to create a user and assign a role
    private async Task<AuthResult> InitializeUser(AppUser user,string password ,string role)
    {
        bool duplicatePhone = await _userManager.Users.AnyAsync(u => u.PhoneNumber == user.PhoneNumber);
        
        if (duplicatePhone)
            return AuthResult.Fail("Phone number already exists");
        
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            return AuthResult.Fail(result.Errors.Select(e => e.Description));
        
        result = await _userManager.AddToRoleAsync(user, role);
        if (!result.Succeeded)
            return AuthResult.Fail(result.Errors.Select(e => e.Description));
        
        return await CreateAuthResultAsync(user, role);
    }
    
    private async Task<AuthResult> CreateAuthResultAsync(AppUser user, string role)
    {
        var (refreshToken, expiryTime) = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = expiryTime;
        await _userManager.UpdateAsync(user);
        
        return AuthResult.Success(user.Id, _jwtTokenGenerator.GenerateToken(user), refreshToken, expiryTime, role);
    }
    
    private (string token, DateTime ExpireOn) GenerateRefreshToken()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return (Convert.ToBase64String(randomBytes), DateTime.UtcNow.AddDays(7) );
    }
    

    #endregion
}