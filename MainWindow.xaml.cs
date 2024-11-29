using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VideojetApp.ViewModel;

namespace VideojetApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LoginViewModel loginViewModel;
        public MainWindow()
        {
            InitializeComponent();
            loginViewModel = new LoginViewModel();
            this.DataContext = loginViewModel;
        }

        private bool isPasswordVisible = false;

        private void TogglePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (isPasswordVisible)
            {
                // Hide the plain text password, show the PasswordBox
                PasswordTextBox.Visibility = Visibility.Collapsed;
                PasswordBox.Visibility = Visibility.Visible;
                PasswordBox.Password = PasswordTextBox.Text; // Sync the password text
                isPasswordVisible = false;


            }
            else
            {
                // Show the plain text password, hide the PasswordBox
                PasswordTextBox.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Collapsed;
                PasswordTextBox.Text = PasswordBox.Password; // Sync the password text
                isPasswordVisible = true;


            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel viewModel)
            {
                viewModel.CurrentUser.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}
