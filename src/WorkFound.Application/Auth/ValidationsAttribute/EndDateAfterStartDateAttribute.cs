using System.ComponentModel.DataAnnotations;

namespace WorkFound.Application.Auth.ValidationsAttribute;

public class EndDateAfterStartDateAttribute : ValidationAttribute
{
    private readonly string _startDatePropertyName;

    public EndDateAfterStartDateAttribute(string startDatePropertyName)
    {
        _startDatePropertyName = startDatePropertyName;
    }
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var endDate = value as DateTime?;
        if (endDate == null)
        {
            return ValidationResult.Success; // No end date provided, validation passes
        }
        
        var startDateProperty = validationContext.ObjectType.GetProperty(_startDatePropertyName);
        if (startDateProperty == null)
        {
            return new ValidationResult($"Unknown property: {_startDatePropertyName}");
        }
        var startDate = startDateProperty.GetValue(validationContext.ObjectInstance) as DateTime?;
        if (startDate.HasValue  && endDate.Value < startDate.Value)
        {
            return new ValidationResult($"End date must be after start date.");
        }
        return ValidationResult.Success; // Validation passes if end date is after start date
    }
}