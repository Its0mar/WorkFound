namespace WorkFound.Domain.Entities.Profile.User;

public class UserEducation
{
    public Guid Id { get; set; }
    public required string SchoolName { get; set; }
    public required string Degree { get; set; }
    public required string FieldOfStudy { get; set; }
    public required DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    public bool IsPresent => EndDate == null;
    
    public Guid UserProfileId { get; set; }
    public UserProfile? UserProfile { get; set; }   
}