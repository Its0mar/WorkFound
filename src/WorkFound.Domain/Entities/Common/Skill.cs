using WorkFound.Domain.Entities.Jobs;
using WorkFound.Domain.Entities.Profile.User;

namespace WorkFound.Domain.Entities.Common;

public class Skill
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    
    public List<UserProfile> UserProfiles { get; set; } = new();
    public List<JobPost> JobPosts { get; set; } = new();
}