using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using pract7.Models;

namespace pract7.Converters
{
    public class DaysSinceLastAppointmentConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var appointments = value as System.Collections.ObjectModel.ObservableCollection<Appointment>;

            if (appointments == null || appointments.Count == 0)
            {
                return "Первый прием в клинике";
            }

            try
            {
                var lastAppointment = appointments.Where(a => a.Date != DateTime.Now.ToString("dd.MM.yyyy")).OrderByDescending(a => DateTime.ParseExact(a.Date, "dd.MM.yyyy", CultureInfo.InvariantCulture)).FirstOrDefault();

                if (lastAppointment != null)
                {
                    DateTime lastDate = DateTime.ParseExact(lastAppointment.Date, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    int days = (DateTime.Now - lastDate).Days;
                    return $"С последнего приема прошло {days} дней";
                }

                return "Первый прием в клинике";
            }
            catch
            {
                return "Первый прием в клинике";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}