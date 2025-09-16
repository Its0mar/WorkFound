using WorkFound.Application.Common.Result;

namespace WorkFound.Application.Auth.Interfaces;

public interface ITokenManagementService
{
    Task<AuthResult> RefreshTokenAsync(string token);
    Task<bool> RevokeTokenAsync(string token);
}