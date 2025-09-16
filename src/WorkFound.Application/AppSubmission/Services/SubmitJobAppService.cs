using Microsoft.EntityFrameworkCore;
using WorkFound.Application.AppSubmission.Interfaces;
using WorkFound.Application.Common.Interface;
using WorkFound.Application.Jobs.Dto.Application.Submit;
using WorkFound.Domain.Entities.Jobs.Application.Answers;
using WorkFound.Domain.Entities.Jobs.Application.Forms;

namespace WorkFound.Application.AppSubmission.Services;

public class SubmitJobAppService(IAppDbContext context) : ISubmitJobAppService
{
    public async Task<bool> SubmitJobApplicationAsync(Guid userId, Guid jobId, AppSubmitDto dto)
    {   
        // check if user profile exists and that user hasn't already applied to this job
        if (!GetUserProfileId(userId, out var userProfileId))
            return false;
        if (await HasUserAppliedToJobAsync(dto.FormId, userProfileId))
            return false;
        
        // Fetch job with active form and questions
        var job = await context.Jobs
            .Where(j => j.Id == jobId && j.ActiveFormId == dto.FormId && j.IsOpen && j.IsPublic)
            .Select(j => new
            {
                j.Id,
                ActiveForm = j.ActiveForm == null
                    ? null
                    : new
                    {
                        j.ActiveFormId,
                        Questions = j.ActiveForm.Questions.Select(q => new
                        {
                            q.Id,
                            Options = q.Options.Select(o => new { o.Id, o.Text }).ToList()
                        })
                    }
            }).FirstOrDefaultAsync();
        
        //if job or active form is null or not applicable, return false
        if (job?.ActiveForm == null) return false;
    
        // Create lookups for questions and options to optimize validation
        var questionLookup = job.ActiveForm.Questions.ToDictionary(q => q.Id);
        var optionLookups = job.ActiveForm.Questions
            .Where(q => q.Options?.Any() == true)
            .ToDictionary(q => q.Id, q => q.Options.ToDictionary(o => o.Id));

        // Create submission object
        var submission = ToJobAppSubmitForm(dto, userProfileId);
        var answers = new List<JobAppSubmitAnswer>(dto.Answers.Count);

        // Process answers with validation
        foreach (var answerDto in dto.Answers)
        {
            // Validate question exists
            if (!questionLookup.ContainsKey(answerDto.QuestionId))
                return false;

            var answer = ToJobAppSubmitAnswer(dto, answerDto);

            // Handle option validation if applicable
            var selectedOptionIds = answerDto.SelectedOptionIds;
            if (selectedOptionIds?.Count > 0)
            {
                if (!optionLookups.TryGetValue(answerDto.QuestionId, out var questionOptions))
                    return false;

                // Validate all selected options exist
                if (selectedOptionIds.Any(optionId => !questionOptions.ContainsKey(optionId)))
                    return false;

                // Create answer options in batch
                answer.SelectedOptions = selectedOptionIds
                    .Select(optionId => new JobAppSubmitAnswerOption { OptionId = optionId })
                    .ToList();
            }

            answers.Add(answer);
        }
        
        submission.Answers.AddRange(answers);

        try
        {
            await context.JobAppSubmitForms.AddAsync(submission);
            return await context.SaveChangesAsync() > 0;
        }
        catch (DbUpdateException)
        {
            // Log exception if needed
            return false;
        }
    }

    private JobAppSubmitForm ToJobAppSubmitForm(AppSubmitDto dto, Guid userProfileId)
    {
        return new JobAppSubmitForm
        {
            JobApplicationId = dto.FormId,
            UserProfileId = userProfileId,
            SubmittedAt = DateTime.UtcNow
        };
    }
    
    private JobAppSubmitAnswer ToJobAppSubmitAnswer(AppSubmitDto dto, AppSubmitAnsweDto answerDto)
    {
        return new JobAppSubmitAnswer
        {
            JobAppSubmitFormId = dto.FormId,
            JobApplicationQuestionId = answerDto.QuestionId,
            AnswerText = answerDto.AnswerText,
        };
    }
    
    private bool GetUserProfileId(Guid appUserId, out Guid userProfileId)
        {
            var userProfile = context.UserProfiles.FirstOrDefault(c => c.AppUserId == appUserId);
            if (userProfile is null)
            {
                userProfileId = Guid.Empty;
                return false;
            }
            
            userProfileId = userProfile.Id;
            return true;
        }
    
    private async Task<bool> HasUserAppliedToJobAsync(Guid applictionFormId, Guid userProfileId)
    {
        return await context.JobAppSubmitForms.AnyAsync(ja => ja.JobApplicationId == applictionFormId && ja.UserProfileId == userProfileId);
    }
    
    
}