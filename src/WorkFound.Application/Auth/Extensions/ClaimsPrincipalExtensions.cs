using System.Security.Claims;

namespace WorkFound.Application.Auth.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var userId =  user.FindFirstValue(ClaimTypes.NameIdentifier);
        
        return Guid.Parse(userId ?? Guid.Empty.ToString());
    }
}