using WorkFound.Domain.Entities.Auth;

namespace WorkFound.Application.Auth.TokenGenerator;

public interface IJwtTokenGenerator
{
    string GenerateToken(AppUser user);
}