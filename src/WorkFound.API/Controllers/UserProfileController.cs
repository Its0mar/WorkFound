using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkFound.Application.Auth.Dtos.UserProfileDtos;
using WorkFound.Application.Auth.Extensions;
using WorkFound.Application.User.Services;

namespace WorkFound.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserProfileController : ControllerBase
{
    private readonly IUserService _userService;

    public UserProfileController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost("add-experience")]
    public async Task<IActionResult> AddExperience([FromForm] UserExperinceDto dto)
    {
        var result = await _userService.AddExperince(dto, User.GetUserId());
        if (!result)
            return BadRequest("Failed to add experience.");
        return Ok("Experience added successfully.");
        
    }
    
    [HttpPost("add-education")]
    public async Task<IActionResult> AddEducation([FromForm] UserEducationDto dto)
    {
        var result = await _userService.AddEducation(dto, User.GetUserId());
        if (!result)
            return BadRequest("Failed to add education.");
        return Ok("Education added successfully.");
    }
    
    [HttpPost("add-skill")]
    public async Task<IActionResult> AddSkill([FromForm] UserSkillDto dto)
    {
        var result = await _userService.AddSkill(dto, User.GetUserId());
        if (!result)
            return BadRequest("Failed to add skill.");
        return Ok("Skill added successfully.");
    }
    
    [HttpDelete("remove-experience/{id}")]
    public async Task<IActionResult> RemoveExperience(Guid id)
    {
        var result = await _userService.RemoveExperince(id);
        if (!result)
            return BadRequest("Failed to remove experience.");
        return Ok("Experience removed successfully.");
    }
    
    [HttpDelete("remove-education/{id}")]
    public async Task<IActionResult> RemoveEducation(Guid id)
    {
        var result = await _userService.RemoveEducation(id);
        if (!result)
            return BadRequest("Failed to remove education.");
        return Ok("Education removed successfully.");
    }
    
    [HttpDelete("remove-skill/{id}")]
    public async Task<IActionResult> RemoveSkill(Guid id)
    {
        var result = await _userService.RemoveSkill(id);
        if (!result)
            return BadRequest("Failed to remove skill.");
        return Ok("Skill removed successfully.");
    }
}