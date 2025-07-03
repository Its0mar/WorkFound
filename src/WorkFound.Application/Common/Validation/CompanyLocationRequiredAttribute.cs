using System.ComponentModel.DataAnnotations;
using WorkFound.Application.Auth.Dtos;
using WorkFound.Domain.Entities.Enums;
using WorkFound.Domain.Entities.Profile.Company;

namespace WorkFound.Application.Common.Validation;

public class CompanyLocationRequiredAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        //var dto = validationContext.ObjectInstance as CompanyRegisterDto;
        
        if (validationContext.ObjectInstance is CompanyRegisterDto dto)
        {
            if (dto.LocationType == CompanyLocationType.Physical && string.IsNullOrEmpty(dto.Location))
            {
                return new ValidationResult("Location is required for physical location.");
            }
            
            else if (dto.LocationType == CompanyLocationType.Virtual && !string.IsNullOrEmpty(dto.Location))
            {
                return new ValidationResult("Location should not be provided for virtual company.");
            }
        }
        return ValidationResult.Success;
    }
}