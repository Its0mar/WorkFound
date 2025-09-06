namespace WorkFound.Application.Validation;

public record ValidationResult(bool IsValid, string? ErrorMessage = null);
