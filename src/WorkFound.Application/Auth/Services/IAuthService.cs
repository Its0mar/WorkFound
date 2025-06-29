using WorkFound.Application.Auth.Dtos;
using WorkFound.Application.Common.Result;

namespace WorkFound.Application.Auth.Services;

public interface IAuthService
{
    Task<AuthResult> CompanyRegisterAsync(CompanyRegisterDto dto);
    Task<AuthResult> UserRegisterAsync(UserRegisterDto dto);
}