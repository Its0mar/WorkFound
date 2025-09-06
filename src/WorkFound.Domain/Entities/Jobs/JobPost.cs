using WorkFound.Domain.Entities.Common;
using WorkFound.Domain.Entities.Enums;
using WorkFound.Domain.Entities.Profile.Company;

namespace WorkFound.Domain.Entities.Jobs;

public class JobPost
{
    public Guid Id { get; set; }
    public required string Title { get; set; } 
    public required string Description { get; set; }
    public required string Location { get; set; }
    public JobLocationType LocationType { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsOpen { get; set; } = true;
    public bool IsPublic { get; set; } = true;
    
    public Guid CompanyId { get; set; }
    public CompanyProfile? CompanyProfile { get; set; }
    
    public List<Skill> Skills { get; set; } = new();
    
}

//TODO: Add required skills 