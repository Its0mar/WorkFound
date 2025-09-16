using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkFound.Application.ApplicationForm.Interfaces;
using WorkFound.Application.AppSubmission.Interfaces;
using WorkFound.Application.Auth.Extensions;
using WorkFound.Application.Jobs.Dto;
using WorkFound.Application.Jobs.Dto.Application.Apply;
using WorkFound.Application.Jobs.Dto.Application.Submit;
using WorkFound.Application.Jobs.Interfaces;
using WorkFound.Application.Jobs.Services;

namespace WorkFound.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class JobsController : ControllerBase
{
    private readonly IJobManagementService _jobManagementService;
    private readonly IJobSkillManagementService _jobSkillManagementService;
    private readonly IJobApplicationFormService _jobApplicationFormService;
    private readonly ISubmitJobAppService _submitJobAppService;

    public JobsController(IJobManagementService jobManagementService, IJobSkillManagementService jobSkillManagementService, IJobApplicationFormService jobApplicationFormService , ISubmitJobAppService submitJobAppService)
    {
        _jobManagementService = jobManagementService;
        _jobSkillManagementService = jobSkillManagementService;
        _jobApplicationFormService = jobApplicationFormService;
        _submitJobAppService = submitJobAppService;
    }
    
    [HttpGet]
    public IActionResult GetJob(Guid id) => Ok(id);
    
    [HttpPost]
    [Authorize(Roles = "Company"), Route("create-job")]
    public async Task<IActionResult> CreateJob([FromForm] AddJobPostDto postDto)
    {
        if (!await _jobManagementService.AddJobAsync(postDto, User.GetUserId()))
            return BadRequest("Failed to create job.");
                
        return Ok("Job created successfully!");
    }
    
    [HttpDelete, Authorize(Roles = "Company"), Route("delete-job/{jobId:guid}")]
    public async Task<IActionResult> DeleteJob(Guid jobId)
    {
        if (!await _jobManagementService.DeleteJobAsync(jobId, User.GetUserId()))
            return BadRequest("Failed to delete job.");
        
        return Ok("Job deleted successfully!");
    }
    
    [HttpPatch, Authorize(Roles = "Company"), Route("open-close-job/{jobId:guid}")]
    public async Task<IActionResult> OpenCloseJob(Guid jobId)
    {
        if (!await _jobManagementService.OpenCloseJobAsync(jobId, User.GetUserId()))
            return BadRequest("Failed to open/close job.");
        
        return Ok("Job status updated successfully!");
    }
    
    [HttpPatch, Authorize(Roles = "Company"), Route("show-hide-job/{jobId:guid}")]
    public async Task<IActionResult> ShowHideJob(Guid jobId)
    {
        if (!await _jobManagementService.ShowHideJobAsync(jobId, User.GetUserId()))
            return BadRequest("Failed to show/hide job.");
        
        return Ok("Job visibility updated successfully!");
    }
    
    [HttpPut, Authorize(Roles = "Company"), Route("update-job/{jobId:guid}")] 
    public async Task<IActionResult> UpdateJob(Guid jobId, [FromForm] UpdateJobDto dto)
    {
        if (!await _jobManagementService.UpdateJobAsync(dto, jobId, User.GetUserId()))
            return BadRequest("Failed to update job.");
        
        return Ok("Job updated successfully!");
    }
    
    //add job skill
    [HttpPut, Authorize(Roles = "Company"), Route("add-skill-job")] 
    public async Task<IActionResult> AddJobSkill([FromForm] AddJobSkillDto dto)
    {
        var companyId = User.GetUserId();
        var result = await _jobSkillManagementService.AddJobSkillAsync(dto, companyId);
        if (!result)
            return BadRequest("Failed to add job skill.");
        return Ok("Job skill added successfully.");
    }

    [HttpDelete, Authorize(Roles = "Company"), Route("remove-skill-job")]
    public async Task<IActionResult> DeleteJobSkill([FromForm] RemoveJobSkillDto dto)
    {
        var companyId = User.GetUserId();
        var result = await _jobSkillManagementService.RemoveJobSkillAsync(dto, companyId);
        if (!result)
            return BadRequest("Failed to remove job skill.");
        return Ok("Job skill removed successfully.");
    }
    
    // [HttpGet, Authorize, Route("show-job/{jobId:guid}")]
    // public async Task<IActionResult> ShowJob(Guid jobId)
    // {
    //     var job = await _jobManagementService.GetPublicJobPostByIdAsync(jobId);
    //     if (job is null)
    //         return NotFound("Job not found.");
    //     return Ok(job);
    // }
    
    [HttpPost, Authorize(Roles = "Company"), Route("create-job-app/{jobId:guid}")]
    
    public async Task<IActionResult> CreateJobApplicationForm([FromBody] CreateJobApplicationFormDto dto)
    {
        if (!await _jobApplicationFormService.CreateJobApplicationFormAsync(dto, User.GetUserId()))
            return BadRequest("Failed to create job application form.");
                
        return Ok("Job application form created successfully!");
    }
    
    [HttpPost, Authorize(Roles = "User"), Route("submit-job-app/{jobId:guid}")]
    public async Task<IActionResult> SubmitJobApplicationForm(Guid jobId, [FromBody] AppSubmitDto dto)
    {
        var userId = User.GetUserId();
        if (!await _submitJobAppService.SubmitJobApplicationAsync(userId, jobId, dto))
            return BadRequest("Failed to submit job application.");
                
        return Ok("Job application submitted successfully!");
    }

        
}