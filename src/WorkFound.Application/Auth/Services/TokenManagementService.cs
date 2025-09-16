using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkFound.Application.Auth.Interfaces;
using WorkFound.Application.Common.Result;
using WorkFound.Domain.Entities.Auth;

namespace WorkFound.Application.Auth.Services;

public class TokenManagementService : ITokenManagementService
{
    private readonly UserManager<AppUser> _userManager;

    public TokenManagementService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<AuthResult> RefreshTokenAsync(string token)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == token);

        if (user is null || !user.IsTokenActive)
            return AuthResult.Fail("Invalid token");

        return await CreateAuthResultAsync(user, user.AccountType.ToString());
    }

    public async Task<bool> RevokeTokenAsync(string token)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == token);
        if (user is null || user.IsTokenActive) return false;

        user.IsTokenRevoked = true;

        return (await _userManager.UpdateAsync(user)).Succeeded;
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