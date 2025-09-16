using Microsoft.AspNetCore.Mvc;
using WorkFound.Application.Auth.Interfaces;
using WorkFound.Application.Auth.Services;
using WorkFound.Application.Common.Interface;
using WorkFound.Application.Common.Result;

namespace WorkFound.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class EmailConfirmationController : ControllerBase
{
    private readonly IEmailConfirmationService _emailConfirmationService;
    private readonly IEmailManagementService _emailManagementService;
    private readonly ICurrentUserService _currentUserService;

    public EmailConfirmationController(IEmailConfirmationService emailConfirmationService, 
        IEmailManagementService emailManagementService,ICurrentUserService currentUserService)
    {
        _emailConfirmationService = emailConfirmationService ?? throw new ArgumentNullException(nameof(emailConfirmationService));
        _emailManagementService = emailManagementService ?? throw new ArgumentNullException(nameof(emailManagementService));
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
    }

    [HttpGet]
    public async Task<ActionResult> RequestEmailConfirmation()
    {
        var user = await _currentUserService.GetCurrentUserAsync();
        if (user is null) return Unauthorized("User not found!");

        await _emailManagementService.SendConfirmationEmailAsync(user, $"{Request.Scheme}://{Request.Host}", "confirm-email");
        return Ok("Confirmation email sent successfully!");
    }

    [HttpPost]
    public async Task<ActionResult<AuthResult>> ConfirmEmail([FromQuery] string userId, string token)
    {
        if (!Guid.TryParse(userId, out var guidUserId))
            return BadRequest("Invalid userId");

        var result = await _emailConfirmationService.ConfirmEmailAsync(guidUserId, token);
        return result.Succeeded ? Ok(result) : BadRequest(result.Errors);
    }
}