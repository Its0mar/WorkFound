using WorkFound.Domain.Entities.Auth;
using WorkFound.Domain.Entities.Profile.Company;

namespace WorkFound.Domain.Entities.Profile.User;

public class UserExperience
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public required string CompanyName { get; set; }
    public Guid? CompanyId { get; set; }
    public bool IsPresent => EndDate == null;
    
    public Guid UserProfileId { get; set; }
    public UserProfile? UserProfile { get; set; }
    public CompanyProfile? CompanyProfile { get; set; }
}

//TODO : add location , duaration in dto or here if needed
//TODO: store company id
//TODO: upload cv
// TODO: upload profile photo
// TODO: save jobs, posts, history of applied jobs