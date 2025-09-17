using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using WorkFound.Application.Auth.Interfaces;
using WorkFound.Application.Common.Interface;
using WorkFound.Application.Common.Result;
using WorkFound.Domain.Entities.Auth;

namespace WorkFound.Application.Auth.Services;

public class EmailManagementService : IEmailManagementService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMailService _mailService;

    public EmailManagementService(UserManager<AppUser> userManager, IMailService mailService)
    {
        _userManager = userManager;
        _mailService = mailService;
    }

    public async Task SendConfirmationEmailAsync(AppUser appUser, string origin, string action)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var confirmationLink = $"{origin}/api/auth/{action}?userId={appUser.Id}&token={encodedToken}";
        
        var subject = "Confirm your email";
        var body = $"Please confirm your email by clicking this link: <a href='{confirmationLink}'>Confirm Email</a>";
        
        await _mailService.SendEmailAsync(appUser.Email!, subject, body);
    }
    public async Task SendResetPasswordEmailAsync(string email, string resetLink)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) return;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetUrl = $"{resetLink}?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(email)}";
        var subject = "Reset your password";
        var body = $"Please reset your password by clicking this link: <a href='{resetUrl}'>Reset Password</a>";

        await _mailService.SendEmailAsync(user.Email!, subject, body);
    }
    
}