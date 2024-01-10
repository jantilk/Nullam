using System.ComponentModel.DataAnnotations;

namespace Application.ValidationAttributes;

public class DateRange : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var startDateProperty = validationContext.ObjectType.GetProperty("StartDate");
        var endDateProperty = validationContext.ObjectType.GetProperty("EndDate");

        if (startDateProperty == null || endDateProperty == null)
        {
            return new ValidationResult("Start and end date properties are required.");
        }

        var startDate = startDateProperty.GetValue(validationContext.ObjectInstance) as DateTime?;
        var endDate = endDateProperty.GetValue(validationContext.ObjectInstance) as DateTime?;

        if (startDate.HasValue && endDate.HasValue && endDate < startDate)
        {
            return new ValidationResult("EndDate must be greater than or equal to StartDate");
        }

        return ValidationResult.Success;
    }
}
