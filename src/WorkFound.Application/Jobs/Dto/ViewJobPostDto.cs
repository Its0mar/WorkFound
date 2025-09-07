namespace WorkFound.Application.Jobs.Dto;

public record ViewJobPostDto
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string LocationType { get; set; }
    public string? Location { get; set; }
    public required bool IsOpen { get; set; }
    public required bool IsPublic { get; set; }
    public required DateTime CreatedAt { get; set; }
    
    public required String CompanyName { get; set; }
    public string? CompanyLogoUrl { get; set; }
    public required List<string> Skills { get; set; }
    
}