using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using pract7.Models;

namespace pract7.Pages
{
    public partial class EditPatientPage :Page
    {
        private Patient patient;

        public event Action PatientUpdated;

        public EditPatientPage(Patient patient)
        {
            InitializeComponent();
            this.patient = patient;

            DataContext = patient;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (Validation.GetHasError(LastNameBox) || Validation.GetHasError(FirstNameBox) || Validation.GetHasError(PhoneBox))
            {
                MessageBox.Show("Исправьте ошибки в форме");
                return;
            }

            patient.LastName = LastNameBox.Text;
            patient.Name = FirstNameBox.Text;
            patient.MiddleName = MiddleNameBox.Text;
            patient.Birthday = BirthdayBox.Text;
            patient.PhoneNumber = PhoneBox.Text;

            string json = JsonSerializer.Serialize(patient);
            string patientId = FindPatientId();
            File.WriteAllText($"P_{patientId}.json", json);

            MessageBox.Show("Сохранено");

            PatientUpdated?.Invoke();

            NavigationService.GoBack();
        }

        private string FindPatientId()
        {
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "P_*.json");
            foreach (string file in files)
            {
                string json = File.ReadAllText(file);
                Patient patientFromFile = JsonSerializer.Deserialize<Patient>(json);
                if (patientFromFile != null &&
                    patientFromFile.LastName == patient.LastName &&
                    patientFromFile.Name == patient.Name)
                {
                    return Path.GetFileNameWithoutExtension(file).Substring(2);
                }
            }
            return "";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}