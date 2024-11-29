using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VideojetApp.Command;
using VideoJetApp.ViewModels;

namespace VideojetApp.ViewModel
{
    public class SettingsViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly ConnectionService connectionService;
        private Window _currentWindow;
        private TcpClient _client;



        public ICommand ConnectCommand { get; }

        private string ipAddress;
        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; OnPropertyChanged(nameof(IpAddress)); }
        }

        private int port;
        public int Port
        {
            get { return port; }
            set { port = value; OnPropertyChanged(nameof(Port)); }
        }

        private string connectionstatus;
        public string ConnectionStatus
        {
            get { return connectionstatus; }
            set { connectionstatus = value; OnPropertyChanged(nameof(ConnectionStatus)); }
        }

        public SettingsViewModel()
        {
            //connectionService = new ConnectionService();
            

            ConnectCommand = new RelayCommand(ConnectToServer);
            ConnectionStatus = "Disconnected";

            connectionService = App.ConnectionService;

        }

        private void ConnectToServer()
        {
            bool isConnected = connectionService.Connect(IpAddress, Port);
            ConnectionStatus = connectionService.ConnectionStatus;

            if (isConnected)
            {
                ConnectionStatus = "Connected";

            }
            else
            {

                ConnectionStatus = "Failed to connect";
            }
        }

        public bool IsConnected => connectionService.IsConnected;
    }
}
