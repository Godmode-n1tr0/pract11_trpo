using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace pract7.Converters
{
    public class PhoneNumberConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string phoneNumber && !string.IsNullOrEmpty(phoneNumber))
            {
                string digits = new string(phoneNumber.Where(char.IsDigit).ToArray());

                if (digits.Length == 11)
                {
                    return $"+7 ({digits.Substring(1, 3)}) {digits.Substring(4, 3)}-{digits.Substring(7, 2)}-{digits.Substring(9, 2)}";
                }
                else if (digits.Length == 10)
                {
                    return $"+7 ({digits.Substring(0, 3)}) {digits.Substring(3, 3)}-{digits.Substring(6, 2)}-{digits.Substring(8, 2)}";
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}