using System.ComponentModel.DataAnnotations;

namespace Application.ValidationAttributes
{
    // TODO: check compiler warnings
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DateInTheFutureAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            DateTime futureDate = DateTime.Parse(value.ToString());
            var memberNames = new List<string>() { context.MemberName };

            if (futureDate < DateTime.Now)
            {
                return new ValidationResult("This must be a date in the future", memberNames);
            }

            return ValidationResult.Success;
        }
    }
}
