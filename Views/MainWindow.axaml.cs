using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using Csv;
using Avalonia.Controls;
using Avalonia.Interactivity;
using lotusctl.Library;

namespace lotusctl.Views {
    public partial class MainWindow : Window {
        private List<Service> _services = new List<Service>();
        private Service? _serviceSelected = null;

        public MainWindow() {
            InitializeComponent();
            LoadConfiguration();
        }
        private async void OnWindowLoad(object? sender, EventArgs e) {
            EnableButtons(false);
            await LoadServicesData();
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
        private async void OnBtnClick(object? sender, RoutedEventArgs e) {
            Pause();
            var button = sender != null ? (Button)sender : null;
            switch (button?.Name) {
                case "BtnStart":
                    await OnBtnStartClick(sender, e);
                    break;
                case "BtnStop":
                    await OnBtnStopClick(sender, e);
                    break;
                case "BtnRestart":
                    await OnBtnRestartClick(sender, e);
                    break;
                case "BtnStatus":
                    await OnBtnStatusClick(sender, e);
                    break;
            }
            Resume();
        }
        private async Task OnBtnStartClick(object? sender, RoutedEventArgs e) {
            if (_serviceSelected == null) return;
            TxtOutput.Text = await Systemd.Start(_serviceSelected.CodeName);
            await LoadServicesData();
        }
        private async Task OnBtnStopClick(object? sender, RoutedEventArgs e) {
            if (_serviceSelected == null) return;
            TxtOutput.Text = await Systemd.Stop(_serviceSelected.CodeName);
            await LoadServicesData();
        }
        private async Task OnBtnRestartClick(object? sender, RoutedEventArgs e) {
            if (_serviceSelected == null) return;
            TxtOutput.Text = await Systemd.Restart(_serviceSelected.CodeName);
            await LoadServicesData();
        }
        private async Task OnBtnStatusClick(object? sender, RoutedEventArgs e) {
            if (_serviceSelected == null) return;
            TxtOutput.Text = await Systemd.Status(_serviceSelected.CodeName);
        }
        private async void OnBtnRefreshClick(object? sender, RoutedEventArgs e) {
            Pause();
            await LoadServicesData();
            TxtOutput.Text = "";
            LstService.IsEnabled = true;
            BtnRefresh.IsEnabled = true;
        }

        private void EnableButtons(bool enable = true) =>
            BtnStart.IsEnabled =
            BtnStop.IsEnabled =
            BtnRestart.IsEnabled =
            BtnStatus.IsEnabled =
            enable;

        private async Task LoadServicesData() {
            await Systemd.Load(_services);
            LstService.Items = _services
                .Select(s => $"{(s.IsActive ? "🍏" : "🍎")}  {s.DisplayName}")
                .ToArray();
        }
        private void LoadConfiguration() {
            var dirName = $"/home/{Environment.UserName}/.config/lotusctl";
            var fileName = $"{dirName}/services.csv";
            if (!File.Exists(fileName)) {
                if (!Directory.Exists(dirName)) {
                    Directory.CreateDirectory(dirName);
                }
                File.Copy("./services.csv", fileName);
            }

            foreach (var line in CsvReader.ReadFromText(File.ReadAllText(fileName))) {
                _services.Add(new Service(line[0], line[1]));
            }
        }

        private void Pause() {
            LstService.IsEnabled = false;
            BtnRefresh.IsEnabled = false;
            TxtOutput.Text = "...";
            EnableButtons(false);
        }
        private void Resume() {
            LstService.IsEnabled = true;
            BtnRefresh.IsEnabled = true;
            EnableButtons(true);
        }
    }
}