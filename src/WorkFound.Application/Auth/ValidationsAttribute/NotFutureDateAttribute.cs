using System.ComponentModel.DataAnnotations;

namespace WorkFound.Application.Auth.ValidationsAttribute;

public class NotFutureDateAttribute : ValidationAttribute 
{
    public override bool IsValid(object? value)
    {
        if (value is not DateTime date) return false;
        if (date > DateTime.Now)
        {
            ErrorMessage = "The date cannot be in the future.";
            return false;
        }

        return true;
    }
}