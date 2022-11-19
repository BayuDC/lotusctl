using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using lotusctl.Library;

namespace lotusctl {
    public partial class MainWindow : Window {
        private List<Service> _services;
        private Service? _serviceSelected;

        public MainWindow() {
            InitializeComponent();
            _services = new List<Service> {
                new Service("apache2", "Apache"),
                new Service("nginx", "Nginx"),
                new Service("mariadb", "MariaDB"),
                new Service("postgresql", "PostgreSQL"),
                new Service("docker", "Docker"),
            };
        }
        private void OnWindowLoad(object? sender, EventArgs e) {
            Systemd.Load(_services);
            LstService.Items = _services
                .Select(s => $"{(s.IsActive ? "🍏" : "🍎")}  {s.DisplayName}")
                .ToArray();
            EnableButtons(false);
        }
        private void OnLstServiceSelect(object? sender, SelectionChangedEventArgs e) {
            var index = LstService.SelectedIndex;
            if (index < 0) {
                EnableButtons(false);
                _serviceSelected = null;
                return;
            }
            EnableButtons();
            _serviceSelected = _services[index];

        }
        private async void OnBtnStartClick(object? sender, RoutedEventArgs e) {
            if (_serviceSelected == null) return;
            TxtOutput.Text = await Systemd.Start(_serviceSelected.CodeName);
        }
        private async void OnBtnStopClick(object? sender, RoutedEventArgs e) {
            if (_serviceSelected == null) return;
            TxtOutput.Text = await Systemd.Stop(_serviceSelected.CodeName);
        }
        private async void OnBtnRestartClick(object? sender, RoutedEventArgs e) {
            if (_serviceSelected == null) return;
            TxtOutput.Text = await Systemd.Restart(_serviceSelected.CodeName);
        }
        private async void OnBtnStatusClick(object? sender, RoutedEventArgs e) {
            if (_serviceSelected == null) return;
            TxtOutput.Text = await Systemd.Status(_serviceSelected.CodeName);
        }

        private void EnableButtons(bool enable = true) {
            BtnStart.IsEnabled = BtnStop.IsEnabled = BtnRestart.IsEnabled = BtnRemove.IsEnabled = enable;
        }
    }
}