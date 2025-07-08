using WorkFound.Application.Auth.Dtos;
using WorkFound.Application.Auth.Dtos.Password;
using WorkFound.Application.Auth.Dtos.Register;
using WorkFound.Application.Common.Result;

namespace WorkFound.Application.Auth.Services;

public interface IAuthService
{
    Task<AuthResult> CompanyRegisterAsync(CompanyRegisterDto dto);
    Task<AuthResult> UserRegisterAsync(UserRegisterDto dto);
    public Task<AuthResult> LoginAsync(LoginDto dto);
    public Task<AuthResult> ChangePasswordAsync(ChangePasswordDto dto, Guid userId);
    public Task<AuthResult> RefreshTokenAsync(string token);
    public Task<bool> RevokeTokenAsync(string token);
    public Task<AuthResult> ConfirmEmailAsync(string token, Guid userId);
    public Task SendResetPasswordEmailAsync(string email, string resetLink);
    public Task<AuthResult> ResetPasswordAsync(ResetPasswordDto dto, string email, string token);

}