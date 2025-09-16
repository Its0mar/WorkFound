using WorkFound.Application.Auth.Dto;
using WorkFound.Application.Auth.Dtos;
using WorkFound.Application.Common.Result;

namespace WorkFound.Application.Auth.Interfaces;

public interface IAuthenticationService
{
    Task<AuthResult> LoginAsync(LoginDto dto);
}