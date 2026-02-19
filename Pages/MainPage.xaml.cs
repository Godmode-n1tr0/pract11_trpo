using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using pract7.Models;

namespace pract7.Pages
{
    public partial class MainPage : Page, INotifyPropertyChanged
    {
        private Doctor currentDoctor;
        private ObservableCollection<Patient> _patients = new ObservableCollection<Patient>();
        private Patient _selectedPatient;
        private int _patientCount;
        private int _doctorCount;

        public ObservableCollection<Patient> Patients
        {
            get => _patients;
            set
            {
                _patients = value;
                OnPropertyChanged();
            }
        }

        public Patient SelectedPatient
        {
            get => _selectedPatient;
            set
            {
                _selectedPatient = value;
                OnPropertyChanged();
            }
        }

        public int PatientCount
        {
            get => _patientCount;
            set
            {
                _patientCount = value;
                OnPropertyChanged();
            }
        }

        public int DoctorCount
        {
            get => _doctorCount;
            set
            {
                _doctorCount = value;
                OnPropertyChanged();
            }
        }

        public MainPage(Doctor doctor)
        {
            InitializeComponent();
            currentDoctor = doctor;
            DataContext = this;
            LoadDoctorInfo();

            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAllData();
        }

        private void LoadAllData()
        {
            LoadCounts();
            LoadPatients();
        }

        private void LoadDoctorInfo()
        {
            DoctorLabel.Content = $"{currentDoctor.LastName} {currentDoctor.Name} {currentDoctor.MiddleName}";
            SpecLabel.Content = currentDoctor.Specialization;
        }

        private void LoadCounts()
        {
            string[] doctorFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "D_*.json");
            DoctorCount = doctorFiles.Length;

            string[] patientFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "P_*.json");
            PatientCount = patientFiles.Length;
        }

        private void LoadPatients()
        {
            Patients.Clear();

            string[] patientFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "P_*.json");

            foreach (string file in patientFiles)
            {
                string json = File.ReadAllText(file);
                Patient patient = JsonSerializer.Deserialize<Patient>(json);
                if (patient != null)
                {
                    Patients.Add(patient);
                }
            }
        }

        private void CreatePatient_Click(object sender, RoutedEventArgs e)
        {
            CreatePatientPage createPage = new CreatePatientPage();
            createPage.PatientCreated += OnPatientCreated;
            NavigationService.Navigate(createPage);
        }

        private void OnPatientCreated()
        {
            LoadAllData();
        }

        private void StartAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPatient != null)
            {
                NavigationService.Navigate(new AppointmentPage(currentDoctor, SelectedPatient));
            }
            else
            {
                MessageBox.Show("Выберите пациента");
            }
        }

        private void EditPatient_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPatient != null)
            {
                EditPatientPage editPage = new EditPatientPage(SelectedPatient);
                editPage.PatientUpdated += OnPatientUpdated;
                NavigationService.Navigate(editPage);
            }
            else
            {
                MessageBox.Show("Выберите пациента");
            }
        }

        private void DeletePatient_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPatient != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить пациента {SelectedPatient.LastName} {SelectedPatient.Name}?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    DeletePatientFile(SelectedPatient);
                    LoadAllData();
                    MessageBox.Show("Пациент удален");
                }
            }
            else
            {
                MessageBox.Show("Выберите пациента для удаления");
            }
        }

        private void DeletePatientFile(Patient patient)
        {
            string[] patientFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "P_*.json");

            foreach (string file in patientFiles)
            {
                string json = File.ReadAllText(file);
                Patient patientFromFile = JsonSerializer.Deserialize<Patient>(json);

                if (patientFromFile != null &&
                    patientFromFile.LastName == patient.LastName &&
                    patientFromFile.Name == patient.Name &&
                    patientFromFile.PhoneNumber == patient.PhoneNumber)
                {
                    File.Delete(file);
                    break;
                }
            }
        }

        private void OnPatientUpdated()
        {
            LoadPatients();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}