using System.ComponentModel.DataAnnotations;

namespace Domain.ValidationAttributes;

public class EstonianIdCode : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var idCode = value as string;

        if (string.IsNullOrEmpty(idCode))
        {
            return new ValidationResult("Estonian ID code is required.");
        }

        if (!IsValidEstonianIdCode(idCode))
        {
            return new ValidationResult("Invalid Estonian ID code.");
        }

        return ValidationResult.Success;
    }

    private static bool IsValidEstonianIdCode(string idCode)
    {
        if (idCode.Length != 11 || !idCode.All(char.IsDigit))
        {
            return false;
        }

        int[] multipliers1 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1 };
        int[] multipliers2 = { 3, 4, 5, 6, 7, 8, 9, 1, 2, 3 };

        var sum1 = CalculateSum(idCode, multipliers1);
        var checksum = sum1 % 11;

        if (checksum < 10)
        {
            return checksum == int.Parse(idCode[10].ToString());
        }

        var sum2 = CalculateSum(idCode, multipliers2);
        checksum = sum2 % 11;
        if (checksum == 10)
        {
            checksum = 0;
        }
        return checksum == int.Parse(idCode[10].ToString());
    }

    private static int CalculateSum(string idCode, int[] multipliers)
    {
        var sum = 0;
        for (var i = 0; i < 10; i++)
        {
            sum += int.Parse(idCode[i].ToString()) * multipliers[i];
        }
        return sum;
    }
}