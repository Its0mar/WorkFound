using WorkFound.Application.Common.Result;
using WorkFound.Domain.Entities.Auth;

namespace WorkFound.Application.Auth.Interfaces;

public interface IEmailManagementService
{
    public Task SendConfirmationEmailAsync(AppUser appUser, string origin, string action);
    
    Task SendResetPasswordEmailAsync(string email, string resetLink);
}