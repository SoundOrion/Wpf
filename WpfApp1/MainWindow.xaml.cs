using System;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.ComponentModel;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<ServiceStatus> Services { get; set; } = new();
        private readonly System.Timers.Timer _timer;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            // 初期データ
            Services.Add(new ServiceStatus("Web API", true));
            Services.Add(new ServiceStatus("Database", false));
            Services.Add(new ServiceStatus("Cache", true));

            _timer = new System.Timers.Timer(5000);
            _timer.Elapsed += (s, e) => UpdateStatus();
            _timer.AutoReset = true;
            _timer.Start();
        }

        private void UpdateStatus()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var random = new Random();
                foreach (var service in Services)
                {
                    service.IsRunning = random.Next(0, 2) == 0;
                }
            });
        }
    }

    public class ServiceStatus : INotifyPropertyChanged
    {
        private bool _isRunning;
        public string Name { get; set; }
        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    OnPropertyChanged(nameof(IsRunning));
                    OnPropertyChanged(nameof(StatusColor));
                    OnPropertyChanged(nameof(StatusText));
                }
            }
        }

        public Brush StatusColor => IsRunning ? new SolidColorBrush(Color.FromRgb(50, 255, 100)) : new SolidColorBrush(Color.FromRgb(255, 50, 50));
        public string StatusText => IsRunning ? "Online" : "Offline";

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ServiceStatus(string name, bool isRunning)
        {
            Name = name;
            IsRunning = isRunning;
        }
    }
}
