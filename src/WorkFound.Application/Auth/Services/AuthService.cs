using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
    private readonly JwtSettings _jwtSettings;

    public AuthService(UserManager<AppUser> userManager, IJwtTokenGenerator jwtTokenGenerator,
        IAppDbContext context, IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _context = context;
        _jwtSettings = jwtSettings.Value;
    }
    
    public async Task<AuthResult> CompanyRegisterAsync(CompanyRegisterDto dto)
    {
        var appUser = dto.ToAppUser(AccountType.Company);
        var result = await InitializeUser(appUser, dto.Password, "Company");
        
        if (!result.Succeeded)
            return result;
        
        var companyProfile = dto.ToCompnyProfile(appUser);
        await _context.CompanyProfiles.AddAsync(companyProfile);
        await _context.SaveChangesAsync();
        
        return result;
    }
    
    public async Task<AuthResult> UserRegisterAsync(UserRegisterDto dto)
    {
        var appUser = dto.ToAppUser(AccountType.User);
        var result = await InitializeUser(appUser, dto.Password, "User");
        
        if (!result.Succeeded)
            return result;
        //appUser.RefreshTokens?.Add(GenerateRefreshToken());
        //await _userManager.UpdateAsync(appUser);
        
        var userProfile = dto.ToUserProfile(appUser);
        await _context.UserProfiles.AddAsync(userProfile);
        await _context.SaveChangesAsync();
        
        return result;
    }
    
    public async Task<AuthResult> GetTokenAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        
        if (user is null)
            return AuthResult.Fail("Email Or Password is incorrect");
        
        var token = _jwtTokenGenerator.GenerateToken(user);
        var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? user.AccountType.ToString();
        
        var result =  AuthResult.Success(user.Id, token, role: role);

        // if (user.RefreshTokens?.Any(x => x.IsActive) ?? false)
        // {
        //     var activeRefreshToken = user.RefreshTokens.FirstOrDefault(x => x.IsActive);
        //     result.RefreshToken = activeRefreshToken?.Token;
        //     result.RefreshTokenExpireOn = activeRefreshToken?.ExpireOn;
        // }
        // else
        // {
        //     var refreshToken = GenerateRefreshToken();
        //     result.RefreshToken = refreshToken.Token;
        //     result.RefreshTokenExpireOn = refreshToken.ExpireOn;
        //     user.RefreshTokens?.Add(refreshToken);
        //     await _userManager.UpdateAsync(user);
        // }

        return result;
    }
    
    public async Task<AuthResult> RefreshTokenAsync(string token)
    {
        throw new NotImplementedException();
        // var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
        //
        // if (user is null)
        //     return AuthResult.Fail("Invalid token");
        //
        // var refreshToken = user.RefreshTokens?.Single(t => t.Token == token);
        //
        // if (refreshToken is null || !refreshToken.IsActive)
        //     return AuthResult.Fail("Invalid or expired refresh token");
        //
        //
        // refreshToken.RevokedOn = DateTime.UtcNow;
        //
        // var newRefreshToken = GenerateRefreshToken();
        // var jwtToken = _jwtTokenGenerator.GenerateToken(user);
        //
        // user.RefreshTokens?.Add(newRefreshToken);
        // await _userManager.UpdateAsync(user);
        //
        // return AuthResult.Success(user.Id, jwtToken, newRefreshToken.Token, newRefreshToken.ExpireOn, user.AccountType.ToString());
    }
    
    public async Task<bool> RevokeTokenAsync(string token)
    {
        // var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
        //
        // if (user == null)
        //     return false;
        //
        // var refreshToken = user.RefreshTokens?.Single(t => t.Token == token);
        //
        // if (!refreshToken.IsActive)
        //     return false;
        //
        // refreshToken.RevokedOn = DateTime.UtcNow;
        //
        // await _userManager.UpdateAsync(user);

        return true;
    }
    
    public async Task<AuthResult> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.EmailOrUsername) ??
                   await _userManager.FindByNameAsync(dto.EmailOrUsername);
        
        if (user == null)
            return AuthResult.Fail("Invalid email or username");

        var result = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!result)
            return AuthResult.Fail("Invalid password");

        var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "User";
        return AuthResult.Success(user.Id, _jwtTokenGenerator.GenerateToken(user), role);
    }
    
    public async Task<AuthResult> ChangePasswordAsync(ChangePasswordDto dto, Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        
        if (user is null)
            return AuthResult.Fail("User not found");
        
        var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
        if (!result.Succeeded)
            return AuthResult.Fail(result.Errors.Select(e => e.Description));

        return AuthResult.Success(userId, token: _jwtTokenGenerator.GenerateToken(user),user.AccountType.ToString());
    }
    
    public async Task<AuthResult> ConfirmEmailAsync(string token, Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        
        if (user is null)
            return AuthResult.Fail("User not found");
        
        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
            return AuthResult.Fail(result.Errors.Select(e => e.Description));

        return AuthResult.Success(userId, token: _jwtTokenGenerator.GenerateToken(user), user.AccountType.ToString());
    }
    
    // public async Task<AuthResult> RefreshTokenAsync(string token, string refreshToken)
    // {
    //     try
    //     {
    //         var userId = GetUserIdFromToken(token);
    //         var user = await _userManager.FindByIdAsync(userId.ToString());
    //         
    //         if (user is null || user.RefreshToken != refreshToken)
    //             return AuthResult.Fail("Invalid token or refresh token");
    //
    //         var jwtToken = _jwtTokenGenerator.GenerateToken(user);
    //         var newRefreshToken = GenerateRefreshToken();
    //         
    //         user.RefreshToken = newRefreshToken;
    //         var result = await _userManager.UpdateAsync(user);
    //         
    //         if (!result.Succeeded)
    //             return AuthResult.Fail(result.Errors.Select(e => e.Description));
    //         
    //         return AuthResult.Success(userId, jwtToken, refreshToken, user.AccountType.ToString());
    //     }
    //     catch (Exception ex)
    //     {
    //         return AuthResult.Fail($"An error occurred while refreshing token: {ex.Message}");
    //     }
    // }

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
        
        return AuthResult.Success(user.Id, _jwtTokenGenerator.GenerateToken(user), role);
    }

    
    private RefreshToken GenerateRefreshToken()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return new RefreshToken()
        {
            Token = Convert.ToBase64String(randomBytes),
            ExpireOn = DateTime.UtcNow.AddDays(15),
            CreatedOn = DateTime.UtcNow
        };
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false, 
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey 
                                                                               ?? throw new Exception("JwtSettings not found"))),
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        var jwtToken = securityToken as JwtSecurityToken;
        if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
    
    private Guid GetUserIdFromToken(string token)
    {
        var principal = GetPrincipalFromExpiredToken(token);
        var userId = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        
        return Guid.Parse(userId ?? Guid.Empty.ToString());
    }

    #endregion
}