using WorkFound.Application.Auth.Dto.Password;
using WorkFound.Application.Common.Result;

namespace WorkFound.Application.Auth.Interfaces;

public interface IPasswordManagementService
{
    Task<AuthResult> ChangePasswordAsync(ChangePasswordDto dto, Guid userId);
    Task<AuthResult> ResetPasswordAsync(ResetPasswordDto dto, string email, string token);
}