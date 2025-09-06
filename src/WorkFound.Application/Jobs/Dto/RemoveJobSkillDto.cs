namespace WorkFound.Application.Jobs.Dto;

public record RemoveJobSkillDto
{
    public required Guid JobId { get; init; }
    public required Guid SkillId { get; init; }
}