using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Csv;
using Avalonia.Controls;
using Avalonia.Interactivity;
using lotusctl.Library;

namespace lotusctl {
    public partial class MainWindow : Window {
        private List<Service> _services = new List<Service>();
        private Service? _serviceSelected = null;

        public MainWindow() {
            InitializeComponent();
            LoadConfiguration();
        }
        private void OnWindowLoad(object? sender, EventArgs e) {
            EnableButtons(false);
            LoadServicesData();
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
            Pause();
            TxtOutput.Text = await Systemd.Start(_serviceSelected.CodeName);
            LoadServicesData();
            Resume();
        }
        private async void OnBtnStopClick(object? sender, RoutedEventArgs e) {
            if (_serviceSelected == null) return;
            Pause();
            TxtOutput.Text = await Systemd.Stop(_serviceSelected.CodeName);
            LoadServicesData();
            Resume();
        }
        private async void OnBtnRestartClick(object? sender, RoutedEventArgs e) {
            if (_serviceSelected == null) return;
            Pause();
            TxtOutput.Text = await Systemd.Restart(_serviceSelected.CodeName);
            LoadServicesData();
            Resume();
        }
        private async void OnBtnStatusClick(object? sender, RoutedEventArgs e) {
            if (_serviceSelected == null) return;
            Pause();
            TxtOutput.Text = await Systemd.Status(_serviceSelected.CodeName);
            Resume();
        }

        private void EnableButtons(bool enable = true) =>
            BtnStart.IsEnabled =
            BtnStop.IsEnabled =
            BtnRestart.IsEnabled =
            BtnStatus.IsEnabled =
            enable;

        private async void LoadServicesData() {
            await Systemd.Load(_services);
            LstService.Items = _services
                .Select(s => $"{(s.IsActive ? "🍏" : "🍎")}  {s.DisplayName}")
                .ToArray();
        }
        private void LoadConfiguration() {
            foreach (var line in CsvReader.ReadFromText(File.ReadAllText("./services.csv"))) {
                _services.Add(new Service(line[0], line[1]));
            }
        }

        private void Pause() {
            LstService.IsEnabled = false;
            TxtOutput.Text = "...";
            EnableButtons(false);
        }
        private void Resume() {
            LstService.IsEnabled = true;
            EnableButtons(true);
        }
    }
}