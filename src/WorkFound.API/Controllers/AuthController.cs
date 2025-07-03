using System.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkFound.Application.Auth.Dtos;
using WorkFound.Application.Auth.Extensions;
using WorkFound.Application.Auth.Services;
using WorkFound.Application.Common.Interface;
using WorkFound.Application.Common.Result;
namespace WorkFound.API.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IEmailConfirmationService _emailConfirmationService;
    private readonly ICurrentUserService _currentUserService;
    public AuthController(IAuthService authService, IEmailConfirmationService emailConfirmationService, ICurrentUserService currentUserService)
    {
        _authService = authService;
        _emailConfirmationService = emailConfirmationService;
        _currentUserService = currentUserService;
    }
    
    [Authorize]
    [HttpGet]
    public IActionResult Index()
    {
        return Ok("AuthController is working!");
    }
    
    [HttpPost]
    public async Task<IActionResult> CompanyRegister([FromForm]CompanyRegisterDto dto)
    {
        var result = await _authService.CompanyRegisterAsync(dto);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<AuthResult>> UserRegister([FromForm]UserRegisterDto dto)
    {
        var result = await _authService.UserRegisterAsync(dto);
        if (!result.Succeeded)
            return BadRequest(result.Errors);
        
        return result;
    }

    [HttpPost]
    public async Task<ActionResult<AuthResult>> Login([FromForm] LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        if (!result.Succeeded)
            return BadRequest(result.Errors);
        
        return result;
    }

    [HttpGet]
    public async Task<ActionResult> RequestEmailConfirmation()
    {
        var user = await _currentUserService.GetCurrentUserAsync();
        if (user is null)
            return Unauthorized("User not found!");

        await _emailConfirmationService.SendConfirmationEmailAsync(user, $"{Request.Scheme}://{Request.Host}", nameof(ConfirmEmail));
        return Ok("Confirmation email sent successfully!");
    }
    
    [HttpGet]
    public async Task<ActionResult<AuthResult>> ConfirmEmail([FromQuery]string userId ,string token)
    {
        var result = await _authService.ConfirmEmailAsync(token, Guid.Parse(userId));
        
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(result);
    }
    
    [Authorize]
    [HttpPatch]
    public async Task<ActionResult<AuthResult>> ChangePassword([FromForm] ChangePasswordDto dto)
    {
        var result = await _authService.ChangePasswordAsync(dto, User.GetUserId());
        if (!result.Succeeded)
            return BadRequest(result.Errors);
        
        return result;
    }

    [HttpPost]
    public async Task<ActionResult> ResetPasswordRequest([FromForm] ResetPasswordRequestDto dto)
    {

        var resetLink = $"{Request.Scheme}://{Request.Host}/api/auth/{nameof(ResetPassword)}";
        await _authService.SendResetPasswordEmailAsync(dto.Email, resetLink);
        
        return Ok("If an account with this email exists, a reset password link has been sent to it.");
    }

    [HttpPost]
    public async Task<ActionResult<AuthResult>> ResetPassword([FromForm] ResetPasswordDto dto, [FromQuery] string token,
        string email)
    {
        var result = await _authService.ResetPasswordAsync(dto, email, token);
        
        if (!result.Succeeded)
            return BadRequest(result.Errors);
        
        return result;
    }

    [HttpGet]
    public async Task<IActionResult> RefreshToken(string refreshToken)
    {
        
        //var refreshToken = Request.Cookies["refreshToken"];
        var result = await _authService.RefreshTokenAsync(refreshToken);

        if (result.RefreshToken is null || result.RefreshTokenExpireOn is null)
            return BadRequest("Invalid refresh token!");
        
        SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpireOn.Value);

        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> RevokeToken(string token)
    {
        var result = await _authService.RevokeTokenAsync(token);

        if(!result)
            return BadRequest("Token is invalid!");

        return Ok();
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


