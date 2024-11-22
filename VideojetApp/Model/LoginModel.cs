using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideojetApp.Model
{
    public class LoginModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged("Id"); }
        }


        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; OnPropertyChanged("Username"); }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged("Password"); }
        }

        private string role;

        public string Role
        {
            get { return role; }
            set { role = value; OnPropertyChanged(Role); }
        }


    }
}
