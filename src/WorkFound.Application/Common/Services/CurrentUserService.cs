using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using WorkFound.Application.Common.Interface;
using WorkFound.Domain.Entities.Auth;

namespace WorkFound.Application.Common.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<AppUser> _userManager;
    
    public CurrentUserService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<AppUser?> GetCurrentUserAsync()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        
        if (user?.Identity?.IsAuthenticated != true)
            return null;
        
        return await _userManager.GetUserAsync(user);
    }
}