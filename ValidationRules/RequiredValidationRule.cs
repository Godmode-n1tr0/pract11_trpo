using System.Globalization;
using System.Windows.Controls;

namespace pract7.ValidationRules
{
    public class RequiredValidationRule :ValidationRule
    {
        public string FieldName { get; set; } = "Поле";

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return new ValidationResult(false, $"{FieldName} обязательно для заполнения");
            }
            return ValidationResult.ValidResult;
        }
    }
}