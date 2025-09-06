using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkFound.Application.Auth.Extensions;
using WorkFound.Application.Jobs.Dto;
using WorkFound.Application.Jobs.Services;

namespace WorkFound.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class JobsController : ControllerBase
{
    private readonly IJobService _jobService;

    public JobsController(IJobService jobService)
    {
        _jobService = jobService;
    }
    
    [HttpGet]
    public IActionResult GetJob(Guid id) => Ok(id);
    
    [HttpPost]
    [Authorize(Roles = "Company"), Route("create-job")]
    public async Task<IActionResult> CreateJob([FromForm] AddJobPostDto postDto)
    {
        if (!await _jobService.AddJobAsync(postDto, User.GetUserId()))
            return BadRequest("Failed to create job.");
                
        return Ok("Job created successfully!");
    }
    
    [HttpDelete, Authorize(Roles = "Company"), Route("delete-job/{jobId:guid}")]
    public async Task<IActionResult> DeleteJob(Guid jobId)
    {
        if (!await _jobService.DeleteJobAsync(jobId, User.GetUserId()))
            return BadRequest("Failed to delete job.");
        
        return Ok("Job deleted successfully!");
    }
    
    [HttpPatch, Authorize(Roles = "Company"), Route("open-close-job/{jobId:guid}")]
    public async Task<IActionResult> OpenCloseJob(Guid jobId)
    {
        if (!await _jobService.OpenCloseJobAsync(jobId, User.GetUserId()))
            return BadRequest("Failed to open/close job.");
        
        return Ok("Job status updated successfully!");
    }
    
    [HttpPatch, Authorize(Roles = "Company"), Route("show-hide-job/{jobId:guid}")]
    public async Task<IActionResult> ShowHideJob(Guid jobId)
    {
        if (!await _jobService.ShowHideJobAsync(jobId, User.GetUserId()))
            return BadRequest("Failed to show/hide job.");
        
        return Ok("Job visibility updated successfully!");
    }
    
    [HttpPut, Authorize(Roles = "Company"), Route("update-job/{jobId:guid}")] 
    public async Task<IActionResult> UpdateJob(Guid jobId, [FromForm] UpdateJobDto dto)
    {
        if (!await _jobService.UpdateJobAsync(dto, jobId, User.GetUserId()))
            return BadRequest("Failed to update job.");
        
        return Ok("Job updated successfully!");
    }
    
    //add job skill
    [HttpPut, Authorize(Roles = "Company"), Route("add-skill-job")] 
    public async Task<IActionResult> AddJobSkill([FromForm] AddJobSkillDto dto)
    {
        var companyId = User.GetUserId();
        var result = await _jobService.AddJobSkillAsync(dto, companyId);
        if (!result)
            return BadRequest("Failed to add job skill.");
        return Ok("Job skill added successfully.");
    }

    [HttpDelete, Authorize(Roles = "Company"), Route("remove-skill-job")]
    public async Task<IActionResult> DeleteJobSkill([FromForm] RemoveJobSkillDto dto)
    {
        var companyId = User.GetUserId();
        var result = await _jobService.RemoveJobSkillAsync(dto, companyId);
        if (!result)
            return BadRequest("Failed to remove job skill.");
        return Ok("Job skill removed successfully.");
    }
    
    //update job skill
    //delete job skill
        
}