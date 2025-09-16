using Microsoft.AspNetCore.Mvc;
using WorkFound.Application.Auth.Interfaces;

namespace WorkFound.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TokenManagementController : ControllerBase
{
    private readonly ITokenManagementService _tokenManagementService;

    public TokenManagementController(ITokenManagementService tokenManagementService)
    {
        _tokenManagementService = tokenManagementService ?? throw new ArgumentNullException(nameof(tokenManagementService));
    }

    [HttpPost]
    public async Task<IActionResult> RevokeToken(string token)
    {
        var result = await _tokenManagementService.RevokeTokenAsync(token);
        return result ? Ok() : BadRequest("Token is invalid!");
    }
}