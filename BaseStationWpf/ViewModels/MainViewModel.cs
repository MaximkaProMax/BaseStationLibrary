using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BaseStationLibrary;
using BaseStationLibrary.Models;
using System.Threading.Tasks;
using System.Windows;

namespace BaseStationWpf
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ApiClient _apiClient = new ApiClient();

        // Входные данные
        public double Area { get; set; }
        public double CoverageArea { get; set; }
        public double R1 { get; set; }
        public double R2 { get; set; }
        public double R3 { get; set; }
        public double Handover { get; set; }
        public double HandoverLimit { get; set; }

        // ID базовой станции для API
        private int _stationId;
        public int StationId
        {
            get => _stationId;
            set { _stationId = value; OnPropertyChanged(); }
        }

        public ObservableCollection<double> BuildCoefficients { get; } =
            new ObservableCollection<double> { 0.7, 0.8, 0.9, 1.0, 1.1 };

        private double _selectedCoefficient = 1.0;
        public double SelectedCoefficient
        {
            get => _selectedCoefficient;
            set { _selectedCoefficient = value; OnPropertyChanged(); }
        }

        // Вывод
        public string ResultR0 { get; set; }
        public string ResultR { get; set; }
        public string ResultL { get; set; }
        public string ResultC { get; set; }
        public string ResultN { get; set; }

        public ICommand CalculateCommand { get; }
        public ICommand LoadHandoverCommand { get; }

        public MainViewModel()
        {
            CalculateCommand = new RelayCommand(Calculate);
            LoadHandoverCommand = new RelayCommand(async () => await LoadHandover());
        }

        private async Task LoadHandover()
        {
            var value = await _apiClient.GetHandoverByStationIdAsync(StationId);

            if (value == null)
            {
                MessageBox.Show("Базовая станция не найдена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Handover = value.Value;
            OnPropertyChanged(nameof(Handover));
        }

        private void Calculate()
        {
            var calculator = new BaseStationCalculator();

            var data = new DistrictData
            {
                Area = Area,
                CoverageArea = CoverageArea,
                Radii = new[] { R1, R2, R3 },
                HandoverAverage = Handover,
                HandoverLimit = HandoverLimit,
                BuildCoefficient = SelectedCoefficient
            };

            var result = calculator.CalculateForDistrict(data);

            ResultR0 = $"R₀: {result.R0:F2} км";
            ResultR = $"R: {result.R:F2} км";
            ResultL = $"L: {result.L:F2} сот";
            ResultC = $"C: {result.C:F2}";
            ResultN = $"N: {result.N:F2} БС";

            OnPropertyChanged(nameof(ResultR0));
            OnPropertyChanged(nameof(ResultR));
            OnPropertyChanged(nameof(ResultL));
            OnPropertyChanged(nameof(ResultC));
            OnPropertyChanged(nameof(ResultN));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}