using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using VideojetApp.Command;
using VideoJetApp.ViewModels;
using VideojetApp.View;

namespace VideojetApp.ViewModel
{
    public class ButtonsControlViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Window _currentWindow;

        private readonly ConnectionService connectionService;

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowSettingsCommand { get; }
        public ICommand SignOutCommand { get; }

        private readonly DashboardViewModel dashboardViewModel;
        private readonly SettingsViewModel settingsViewModel;


        public ButtonsControlViewModel()
        {


            //connectionService = new ConnectionService();

            ShowDashboardCommand = new RelayCommand(ShowDashboard);
            ShowSettingsCommand = new RelayCommand(ShowSettings);


            SignOutCommand = new RelayCommand(SignOut);

            // Default to Dashboard view

            ShowDashboard();

            //dashboardViewModel = new DashboardViewModel();
            //settingsViewModel = new SettingsViewModel();

            //ShowDashboardCommand = new RelayCommand(() => CurrentView = dashboardViewModel);
            //ShowSettingsCommand = new RelayCommand(() => CurrentView = settingsViewModel);

            //CurrentView = dashboardViewModel; // Default view
        }

        private void ShowDashboard()
        {
            CurrentView = new Dashboard1();
            
        }

        private void ShowSettings()
        {
            CurrentView = new Settings();
            
        }

        private void SignOut()
        {

            var result = MessageBox.Show("Are you sure you want to sign out?", "Confirm Sign Out", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var loginWindow = new VideojetApp.MainWindow();
                loginWindow.Show();
            }
            Application.Current.Shutdown();
            
        }
        
    }

}
