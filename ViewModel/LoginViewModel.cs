using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideojetApp.Command;
using VideojetApp.Model;

namespace VideojetApp.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        LoginService LoginService; 
        public LoginViewModel()
        {
            LoginService = new LoginService();
            loginCommand = new RelayCommand(Login);
            currentUser = new LoginModel();

        }

        private LoginModel currentUser;

        public LoginModel CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; OnPropertyChanged("CurrentUser"); }
        }

        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged("Message"); }
        }
        private RelayCommand loginCommand;

        public RelayCommand LoginCommand
        {
            get { return loginCommand; }
        }
        public void Login()
        {
            try
            {
                // Validate credentials via LoginService
                bool islogin = LoginService.Checklogin(CurrentUser);

                if (islogin)
                {
                    Message = "Login Successful";
                    // Navigate to another view or do post-login actions here
                }
                else
                {
                    Message = "Invalid Username or Password";
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;  // Handle any exceptions and display message
            }
        }



    }
}
