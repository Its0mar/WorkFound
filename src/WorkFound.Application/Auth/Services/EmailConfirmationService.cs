using Microsoft.AspNetCore.Identity;
using WorkFound.Application.Auth.Interfaces;
using WorkFound.Application.Common.Interface;
using WorkFound.Application.Common.Result;
using WorkFound.Domain.Entities.Auth;

namespace WorkFound.Application.Auth.Services;

public class EmailConfirmationService : IEmailConfirmationService
{
    private readonly IMailService _mailService;
    private readonly UserManager<AppUser> _userManager;
    
    public EmailConfirmationService(IMailService mailService, UserManager<AppUser> userManager)
    {
        _mailService = mailService;
        _userManager = userManager;
    }
    
    public async Task<AuthResult> ConfirmEmailAsync(Guid userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        
        if (user is null) return AuthResult.Fail("User not found");
        
        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
            return AuthResult.Fail(result.Errors.Select(e => e.Description));

        return AuthResult.Success(userId,role: user.AccountType.ToString());
    }

}