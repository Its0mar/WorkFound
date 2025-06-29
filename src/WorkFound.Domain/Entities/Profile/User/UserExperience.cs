using WorkFound.Domain.Entities.Auth;

namespace WorkFound.Domain.Entities.Profile.User;

public class UserExperience
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public required AppUser AppUser { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public required string CompanyName { get; set; }
    public bool Current { get; set; }
}

//TODO : add location , duaration in dto or here if needed