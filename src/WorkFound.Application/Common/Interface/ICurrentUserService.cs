using WorkFound.Domain.Entities.Auth;

namespace WorkFound.Application.Common.Interface;

public interface ICurrentUserService
{
    public Task<AppUser?> GetCurrentUserAsync();
}