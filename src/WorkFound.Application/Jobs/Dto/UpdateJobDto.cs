using System.ComponentModel.DataAnnotations;
using WorkFound.Domain.Entities.Enums;

namespace WorkFound.Application.Jobs.Dto;

public record UpdateJobDto
{
    [MinLength(3, ErrorMessage = "Title must be at least 3 characters long.")]
    [MaxLength(30, ErrorMessage = "Title cannot exceed 30 characters.")]
    public required string Title { get; init; }
    
    [MinLength(10, ErrorMessage = "Description must be at least 10 characters long.")]
    [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public required string Description { get; init; }
    
    [Required(ErrorMessage = "Location is required. If the job is remote, you can specify 'Remote' or 'Anywhere'.")]
    public required string Location { get; init; }
    [Required(ErrorMessage = "Location type is required.")]
    public JobLocationType LocationType { get; init; } 
}
