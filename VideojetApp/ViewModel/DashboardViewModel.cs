using FileBrowserLibrary;
using System;
using System.ComponentModel;
using System.Data;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Input;
using VideojetApp.Command;
using VideoJetApp.ViewModels;




namespace VideojetApp.ViewModel
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly FileBrowserService fileBrowserService;
        private readonly ConnectionService connectionService;
        private Window _currentWindow;
        private TcpClient _client;


        public ICommand SettingsCommand { get; }
        public ICommand BrowseFileCommand { get; }
        public ICommand SignOutCommand { get; }
        public ICommand ConnectCommand { get; }
        public ICommand StartCommand { get; }
        public ICommand SendCommand { get; }



        private DataTable fileData;
        public DataTable FileData
        {
            get { return fileData; }
            set { fileData = value; OnPropertyChanged(nameof(FileData)); }
        }

        private string fileName;
        public string FileName
        {
            get => fileName;
            set { fileName = value; OnPropertyChanged(nameof(FileName)); }
        }

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


        public DashboardViewModel(Window currentWindow)
        {
            connectionService = new ConnectionService();
            _currentWindow = currentWindow;

            SignOutCommand = new RelayCommand(SignOut);
            SettingsCommand = new RelayCommand(ShowSettings);
            fileBrowserService = new FileBrowserService();
            BrowseFileCommand = new RelayCommand(BrowseFile);
            StartCommand = new RelayCommand(SendStartCommand);
            ConnectCommand = new RelayCommand(ConnectToServer);
            SendCommand = new RelayCommand(SendRowToServer);

            ConnectionStatus = "Disconnected";

        }

        private void ShowSettings()
        {

        }
        private void BrowseFile()
        {

            FileData = fileBrowserService.BrowseAndReadFile();
            FileName = fileBrowserService.SelectedFileName;

            //var dataTable = fileBrowserService.BrowseAndReadFile();
            //if (dataTable != null)
            //{
            //    FileName = fileBrowserService.SelectedFileName;
            //    FileData = dataTable;
            //}
        }

        private void SignOut()
        {

            var result = MessageBox.Show("Are you sure you want to sign out?", "Confirm Sign Out", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var loginWindow = new VideojetApp.MainWindow();
                loginWindow.Show();
            }
            _currentWindow.Close();
        }



        //Connect to server.
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

        //Send Start Command.
        private void SendStartCommand()
        {
            try
            {
                if (connectionService != null && connectionService.IsConnected)
                {
                    string command = "SST|1|<CR>";
                    //connectionService.SendData(command);
                    connectionService.Send(command);
                }
                else
                {
                    MessageBox.Show("Please connect to the server before starting the printer.", "Connection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send command: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Send Excel Data.

        private int currentRowIndex = 0;
        private async void SendRowToServer()
        {
            if (FileData == null || FileData.Rows.Count == 0)
            {
                MessageBox.Show("No data to send. Please load an Excel file first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!connectionService.IsConnected)
            {
                MessageBox.Show("Please connect to the server before sending data.", "Connection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (!FileData.Columns.Contains("Status"))
                {
                    FileData.Columns.Add("Status", typeof(string)); // Add a Status column if not present
                }

                while (currentRowIndex < FileData.Rows.Count)
                {
                    DataRow currentRow = FileData.Rows[currentRowIndex];

                    // Format the row as per the required protocol
                    string message = FormatRowForServer(currentRow);

                    // Send the data to the server
                    connectionService.Send(message);

                    // Wait for the PRC<CR> acknowledgment
                    string response = await connectionService.Receive();  // Await the server response


                    if (response == "PRC")
                    {
                        currentRow["Status"] = "Acknowledged";

                        // Refresh the DataGrid
                        FileData.AcceptChanges();

                        currentRowIndex++;  // Only move to the next row if we received the correct acknowledgment
                    }
                    else
                    {
                        MessageBox.Show("Unexpected response from server: " + response, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    }
                }

                if (currentRowIndex >= FileData.Rows.Count)
                {
                    MessageBox.Show("All rows have been sent.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending row: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Row format 
        private string FormatRowForServer(DataRow row)
        {
            // Format: JDI|VAR1=C1|VAR2=C2|VAR3=C3|<CR>
            StringBuilder formattedRow = new StringBuilder("JDI");
            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                formattedRow.Append($"|VAR{i + 1}={row[i]}");
            }
            formattedRow.Append("|<CR>");
            return formattedRow.ToString();
        }


    }
}

