using System;
using System.Globalization;
using System.Windows.Data;

namespace pract7.Converters
{
    public class AgeConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string birthdayString)
            {
                if (DateTime.TryParseExact(birthdayString, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime birthday))
                {
                    int age = CalculateAge(birthday);
                    bool isAdult = age >= 18;
                    return $"{age} лет ({(isAdult ? "совершеннолетний" : "несовершеннолетний")})";
                }
            }
            return "Дата рождения не указана";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age))
                age--;
            return age;
        }
    }
}