using WorkFound.Domain.Entities.Auth;

namespace WorkFound.Application.Auth.Services;

public interface IEmailConfirmationService
{
    public Task SendConfirmationEmailAsync(AppUser appUser, string origin, string action);
    public Task<bool> ConfirmEmailAsync(Guid userId, string token);
}