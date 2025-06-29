using System.ComponentModel.DataAnnotations;
using WorkFound.Domain.Entities.Profile.Company;

namespace WorkFound.Application.Auth.Dtos;

public class CompanyRegisterDto : RegisterDto
{
    [Required(ErrorMessage = "Company Name is required")]
    [MaxLength(50, ErrorMessage = "Company Name cannot exceed 50 characters")]
    [MinLength(2, ErrorMessage = "Company Name must be at least 2 characters long")]
    public required string CompanyName { get; init; }
    [Required(ErrorMessage = "Description is required")]
    [MaxLength(1500, ErrorMessage = "Description cannot exceed 1500 characters")]
    [MinLength(10, ErrorMessage = "Description must be at least 10 characters long")]
    public required string Description { get; init; }
    [DataType(DataType.Url)]
    public string? Website { get; init; }
    [DataType(DataType.ImageUrl)]
    public string? LogoUrl { get; init; }
    public required CompanyLocationType LocationType { get; init; }
    public string? Location { get; init; }
}