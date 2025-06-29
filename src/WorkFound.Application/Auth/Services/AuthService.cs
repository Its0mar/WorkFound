using Microsoft.AspNetCore.Identity;
using WorkFound.Application.Auth.Dtos;
using WorkFound.Application.Auth.Extensions;
using WorkFound.Application.Auth.TokenGenerator;
using WorkFound.Application.Common.Interface;
using WorkFound.Application.Common.Result;
using WorkFound.Domain.Entities.Auth;
using WorkFound.Domain.Entities.Profile.Company;

namespace WorkFound.Application.Auth.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IAppDbContext _context;

    public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, IJwtTokenGenerator jwtTokenGenerator, IAppDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _context = context;
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
        
        var userProfile = dto.ToUserProfile(appUser);
        await _context.UserProfiles.AddAsync(userProfile);
        await _context.SaveChangesAsync();
        
        return result;
        
        ;
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

    #region Utility Methods
    // a method to create a user and assign a role
    private async Task<AuthResult> InitializeUser(AppUser user,string password ,string role)
    {
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            return AuthResult.Fail(result.Errors.Select(e => e.Description));
        
        result = await _userManager.AddToRoleAsync(user, role);
        if (!result.Succeeded)
            return AuthResult.Fail(result.Errors.Select(e => e.Description));
        
        return AuthResult.Success(user.Id, _jwtTokenGenerator.GenerateToken(user), role);
    }

    #endregion
}