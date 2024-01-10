using System.ComponentModel.DataAnnotations;

namespace Application.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class FutureDate : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            if (value == null || !DateTime.TryParse(value.ToString(), out DateTime futureDate))
            {
                return new ValidationResult("Invalid date format");
            }

            var memberNames = new List<string>();
            if (context.MemberName != null)
            {
                memberNames.Add(context.MemberName);
            }

            if (futureDate < DateTime.UtcNow)
            {
                return new ValidationResult("This must be a date in the future", memberNames);
            }

            return ValidationResult.Success;
        }
    }
}
