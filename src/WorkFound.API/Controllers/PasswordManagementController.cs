using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkFound.Application.Auth.Dto.Password;
using WorkFound.Application.Auth.Extensions;
using WorkFound.Application.Auth.Interfaces;
using WorkFound.Application.Common.Result;

namespace WorkFound.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class PasswordManagementController : ControllerBase
{
    private readonly IPasswordManagementService _passwordManagementService;
    private readonly IEmailManagementService _emailManagementService; // Add this dependency

    public PasswordManagementController(IPasswordManagementService passwordManagementService, IEmailManagementService emailManagementService)
    {
        _passwordManagementService = passwordManagementService ?? throw new ArgumentNullException(nameof(passwordManagementService));
        _emailManagementService = emailManagementService ?? throw new ArgumentNullException(nameof(emailManagementService));
    }

    [Authorize]
    [HttpPatch]
    public async Task<ActionResult<AuthResult>> ChangePassword([FromForm] ChangePasswordDto dto)
    {
        var result = await _passwordManagementService.ChangePasswordAsync(dto, User.GetUserId());
        return result.Succeeded ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpPost]
    public async Task<ActionResult> ResetPasswordRequest([FromBody] string email)
    {
        var resetLink = $"{Request.Scheme}://{Request.Host}/api/password-management/reset-password";
        await _emailManagementService.SendResetPasswordEmailAsync(email, resetLink);
        return Ok("If an account with this email exists, a reset password link has been sent to it.");
    }

    [HttpPost]
    public async Task<ActionResult<AuthResult>> ResetPassword([FromForm] ResetPasswordDto dto, [FromQuery] string token, string email)
    {
        var result = await _passwordManagementService.ResetPasswordAsync(dto, email, token);
        return result.Succeeded ? Ok(result) : BadRequest(result.Errors);
    }
}