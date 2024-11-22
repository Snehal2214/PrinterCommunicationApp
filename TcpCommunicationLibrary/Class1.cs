using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpCommunicationLibrary
{
    public class TcpClientServerManager 
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private TcpListener _server;

        public bool IsConnected => _client?.Connected ?? false;

        

        public async Task ConnectAsync(string ipAddress, int port)
        {
            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(ipAddress, port);
                _stream = _client.GetStream();
            }
            catch (Exception ex)
            {
                throw new Exception($"Client failed to connect: {ex.Message}", ex);
            }
        }

        public async Task SendDataAsync(string data)
        {
            if (!IsConnected) throw new InvalidOperationException("Client is not connected.");

            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                await _stream.WriteAsync(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                throw new Exception($"Client failed to send data: {ex.Message}", ex);
            }
        }

        public async Task<string> ReceiveDataAsync()
        {
            if (!IsConnected) throw new InvalidOperationException("Client is not connected.");

            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                return Encoding.ASCII.GetString(buffer, 0, bytesRead);
            }
            catch (Exception ex)
            {
                throw new Exception($"Client failed to receive data: {ex.Message}", ex);
            }
        }

        
    }
}
