using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkFound.Application.Auth.Dto;
using WorkFound.Application.Auth.Dtos;
using WorkFound.Application.Auth.Interfaces;
using WorkFound.Application.Auth.TokenGenerator;
using WorkFound.Application.Common.Result;
using WorkFound.Domain.Entities.Auth;

namespace WorkFound.Application.Auth.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthenticationService(UserManager<AppUser> userManager, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
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

    private async Task<AuthResult> CreateAuthResultAsync(AppUser appUser, string role)
    {
        var (refreshToken, expiryTime) = GenerateRefreshToken();
        appUser.RefreshToken = refreshToken;
        appUser.RefreshTokenExpiryTime = expiryTime;
        await _userManager.UpdateAsync(appUser);

        return AuthResult.Success(appUser.Id, _jwtTokenGenerator.GenerateToken(appUser), refreshToken, expiryTime, role);
    }

    private (string token, DateTime ExpireOn) GenerateRefreshToken()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return (Convert.ToBase64String(randomBytes), DateTime.UtcNow.AddDays(7));
    }
}