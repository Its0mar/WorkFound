using WorkFound.Application.Common.Result;

namespace WorkFound.Application.Auth.Interfaces;

public interface IEmailConfirmationService
{
    public Task<AuthResult> ConfirmEmailAsync(Guid userId, string token);
}