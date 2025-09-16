using WorkFound.Application.Auth.Dto.Register;
using WorkFound.Application.Common.Result;

namespace WorkFound.Application.Auth.Interfaces;

public interface IUserRegistrationService
{
    Task<AuthResult> CompanyRegisterAsync(CompanyRegisterDto dto);
    Task<AuthResult> UserRegisterAsync(UserRegisterDto dto);
}