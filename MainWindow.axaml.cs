using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using lotusctl.Library;

namespace lotusctl {
    public partial class MainWindow : Window {
        public List<Service> _services;
        public MainWindow() {
            InitializeComponent();
            _services = new List<Service> {
                new Service("apache2", "Apache"),
                new Service("nginx", "Nginx"),
                new Service("mariadb", "MariaDB"),
                new Service("postgresql", "PostgreSQL"),
            };
        }
        private void OnWindowLoad(object? sender, EventArgs e) {
            LstService.Items = _services.Select(s => s.DisplayName).ToArray();
            EnableButtons(false);
        }
        private void OnLstServiceSelect(object? sender, SelectionChangedEventArgs e) {
            var index = LstService.SelectedIndex;
            if (index < 0) {
                EnableButtons(false);
            } else {
                EnableButtons(true);
            }

            Console.WriteLine(index);
        }

        private void EnableButtons(bool enable = true) {
            BtnStart.IsEnabled = BtnStop.IsEnabled = BtnRestart.IsEnabled = BtnRemove.IsEnabled = enable;
        }
    }
}