using System.ComponentModel.DataAnnotations;

namespace WorkFound.Application.Jobs.Dto.Application.Submit;

public record AppSubmitDto
{
    public Guid FormId { get; set; }
    [Required, MinLength(1, ErrorMessage = "At least one answer is required")]
    public List<AppSubmitAnsweDto> Answers { get; set; } = new();
}