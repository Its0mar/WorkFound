using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using WorkFound.Application.Auth.Dto.Password;
using WorkFound.Application.Auth.Interfaces;
using WorkFound.Application.Common.Result;
using WorkFound.Domain.Entities.Auth;

namespace WorkFound.Application.Auth.Services;

public class PasswordManagementService : IPasswordManagementService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenManagementService _tokenManagementService;

    public PasswordManagementService(UserManager<AppUser> userManager, ITokenManagementService tokenManagementService)
    {
        _userManager = userManager;
        _tokenManagementService = tokenManagementService;
    }

    public async Task<AuthResult> ChangePasswordAsync(ChangePasswordDto dto, Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null) return AuthResult.Fail("User not found");

        var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
        if (!result.Succeeded)
            return AuthResult.Fail(result.Errors.Select(e => e.Description));

        await _tokenManagementService.RevokeTokenAsync(user.RefreshToken);
        return AuthResult.Success(userId, token: null, user.AccountType.ToString());
    }

    public async Task<AuthResult> ResetPasswordAsync(ResetPasswordDto dto, string email, string token)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return AuthResult.Fail("User not found");

        var result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);
        if (!result.Succeeded)
            return AuthResult.Fail(result.Errors.Select(e => e.Description));

        await _tokenManagementService.RevokeTokenAsync(user.RefreshToken);
        return await CreateAuthResultAsync(user, user.AccountType.ToString());
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