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

    [HttpPut("update-profile-picture"), Authorize]
    public async Task<IActionResult> UpdateProfilePicture(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
        if (!allowedExtensions.Contains(extension))
            return BadRequest("Invalid file type. Only .jpg, .jpeg, and .png are allowed.");
        
        var result = await  _userService.UpdateProfilePicture(User.GetUserId(), file.OpenReadStream(), file.FileName);
         
        if (!result)
              return BadRequest("Failed to update profile picture.");
       
        return Ok("Profile picture updated successfully.");
    }

    [HttpDelete("delete-profile-picture"), Authorize]
    public async Task<IActionResult> DeleteProfilePicture()
    {
        var id = User.GetUserId();
        var result = await _userService.DeleteProfilePicture(id);
        
        if (!result)
            return BadRequest("Failed to delete profile picture.");
        
        return Ok("Profile picture deleted successfully.");
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
    
    [HttpDelete("remove-experience/{experienceId}")]
    public async Task<IActionResult> RemoveExperience(Guid experienceId)
    {
        var result = await _userService.RemoveExperince(experienceId);
        if (!result)
            return BadRequest("Failed to remove experience.");
        return Ok("Experience removed successfully.");
    }
    
    [HttpDelete("remove-education/{educationId}")]
    public async Task<IActionResult> RemoveEducation(Guid educationId)
    {
        var result = await _userService.RemoveEducation(educationId);
        if (!result)
            return BadRequest("Failed to remove education.");
        return Ok("Education removed successfully.");
    }
    
    [HttpDelete("remove-skill/{skillId}")]
    public async Task<IActionResult> RemoveSkill(Guid skillId)
    {
        var result = await _userService.RemoveSkill(User.GetUserId(), skillId);
        if (!result)
            return BadRequest("Failed to remove skill.");
        return Ok("Skill removed successfully.");
    }
}