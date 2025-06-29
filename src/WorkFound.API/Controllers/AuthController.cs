using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WorkFound.Application.Auth;
using WorkFound.Application.Auth.Dtos;
using WorkFound.Application.Auth.Services;
using WorkFound.Application.Auth.TokenGenerator;
using WorkFound.Domain.Entities.Auth;
using WorkFound.Domain.Entities.Profile.Company;

namespace WorkFound.API.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
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
        // check if company is in physical location and location is provided
        if (dto.LocationType == CompanyLocationType.Physical && string.IsNullOrEmpty(dto.Location))
            return BadRequest("Location is required for physical location");
        
        var result = await _authService.CompanyRegisterAsync(dto);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> UserRegister([FromForm]UserRegisterDto dto)
    {
        var result = await _authService.UserRegisterAsync(dto);
        if (!result.Succeeded)
            return BadRequest(result.Errors);
        
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromForm] LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        if (!result.Succeeded)
            return BadRequest(result.Errors);
        
        return Ok(result);
    }
    
}

