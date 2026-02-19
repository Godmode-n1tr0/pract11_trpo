using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace pract7.ValidationRules
{
    public class PhoneNumberValidationRule :ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return ValidationResult.ValidResult;
            }

            string phone = value.ToString();
            string digits = new string(phone.Where(char.IsDigit).ToArray());

            if (digits.Length < 10 || digits.Length > 11)
            {
                return new ValidationResult(false, "Номер телефона должен содержать 10-11 цифр");
            }

            return ValidationResult.ValidResult;
        }
    }
}