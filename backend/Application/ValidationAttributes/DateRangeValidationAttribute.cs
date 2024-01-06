using System.ComponentModel.DataAnnotations;
using Application.DTOs;

namespace Application.ValidationAttributes;

public class DateRangeValidationAttribute : ValidationAttribute
{
    // TODO: check compiler warnings
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var filterDto = (FilterDto)validationContext.ObjectInstance;

        if (filterDto.EndDate < filterDto.StartDate)
        {
            return new ValidationResult("EndDate must be greater than or equal to StartDate");
        }

        return ValidationResult.Success;
    }
}