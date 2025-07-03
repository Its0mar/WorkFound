using Microsoft.AspNetCore.Identity;
using WorkFound.Application.Common.Interface;
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
    
    public async Task SendConfirmationEmailAsync(AppUser user, string origin, string action)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = $"{origin}/api/auth/{action}?userId={user.Id}&token={Uri.EscapeDataString(token)}";
        
        var subject = "Confirm your email";
        var body = $"Please confirm your email by clicking this link: <a href='{confirmationLink}'>Confirm Email</a>";
        
        await _mailService.SendEmailAsync(user.Email!, subject, body);
    }

    public async Task<bool> ConfirmEmailAsync(Guid userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return false;
        
        var result = await _userManager.ConfirmEmailAsync(user, token);
        
        return result.Succeeded;
    }

}