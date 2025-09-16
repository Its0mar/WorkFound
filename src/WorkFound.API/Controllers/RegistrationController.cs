using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkFound.Application.Auth.Dto.Register;
using WorkFound.Application.Auth.Interfaces;
using WorkFound.Application.Common.Result;

namespace WorkFound.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class RegistrationController : ControllerBase
{
    private readonly IUserRegistrationService _userRegistrationService;

    public RegistrationController(IUserRegistrationService userRegistrationService)
    {
        _userRegistrationService = userRegistrationService ?? throw new ArgumentNullException(nameof(userRegistrationService));
    }

    [HttpPost, Authorize(policy: "RequireAnonymous")]
    public async Task<IActionResult> CompanyRegister([FromForm] CompanyRegisterDto dto)
    {
        var result = await _userRegistrationService.CompanyRegisterAsync(dto);
        return result.Succeeded ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpPost, Authorize(policy: "RequireAnonymous")]
    public async Task<ActionResult<AuthResult>> UserRegister([FromForm] UserRegisterDto dto)
    {
        var result = await _userRegistrationService.UserRegisterAsync(dto);
        return result.Succeeded ? Ok(result) : BadRequest(result.Errors);
    }
}