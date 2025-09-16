using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkFound.Application.Auth.Dto;
using WorkFound.Application.Auth.Dtos;
using WorkFound.Application.Auth.Interfaces;
using WorkFound.Application.Common.Result;

namespace WorkFound.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ITokenManagementService _tokenManagementService;

    public AuthenticationController(IAuthenticationService authenticationService, ITokenManagementService tokenManagementService)
    {
        _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        _tokenManagementService = tokenManagementService ?? throw new ArgumentNullException(nameof(tokenManagementService));
    }

    [HttpPost, Authorize(policy: "RequireAnonymous")]
    public async Task<ActionResult<AuthResult>> Login([FromForm] LoginDto dto)
    {
        var result = await _authenticationService.LoginAsync(dto);
        return result.Succeeded ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpPost]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        var result = await _tokenManagementService.RefreshTokenAsync(refreshToken);
        if (!result.Succeeded) return BadRequest(result.Errors);

        SetRefreshTokenInCookie(result.RefreshToken!, result.RefreshTokenExpireOn!.Value);
        return Ok(result);
    }

    private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = expires.ToLocalTime(),
            Secure = true,
            IsEssential = true,
            SameSite = SameSiteMode.None
        };

        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
}