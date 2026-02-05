using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using pract7.Models;

namespace pract7.Pages
{
    public partial class CreatePatientPage :Page
    {
        Random rnd = new Random();
        private Patient patient;

        public event Action PatientCreated;

        public CreatePatientPage()
        {
            InitializeComponent();
            patient = new Patient();
            DataContext = patient;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (Validation.GetHasError(LastNameBox) || Validation.GetHasError(FirstNameBox) || Validation.GetHasError(PhoneBox))
            {
                MessageBox.Show("Исправьте ошибки в форме");
                return;
            }

            if (string.IsNullOrEmpty(patient.LastName) ||
                string.IsNullOrEmpty(patient.Name))
            {
                MessageBox.Show("Заполните фамилию и имя");
                return;
            }

            int patientId = rnd.Next(1000000, 9999999);

            patient.Birthday = BirthdayPicker.SelectedDate?.ToString("dd.MM.yyyy") ?? "";

            string json = JsonSerializer.Serialize(patient);
            File.WriteAllText($"P_{patientId}.json", json);

            MessageBox.Show($"Пациент создан. ID: {patientId}");

            PatientCreated?.Invoke();

            NavigationService.GoBack();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}