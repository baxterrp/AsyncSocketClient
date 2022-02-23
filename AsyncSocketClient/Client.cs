using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AsyncSocketClient
{
    public class Client
    {
        private TcpClient _client;

        public Client()
        {
            _client = new TcpClient();
        }

        public async Task Send(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("You no send blank space - SHAME");
            }

            var writer = new StreamWriter(_client.GetStream());
            writer.AutoFlush = true;

            await writer.WriteAsync(input);
        }

        public async Task Connect()
        {
            try
            {
                var ipAddress = "127.0.0.1";
                var port = 5000;
                var parsedAddress = IPAddress.Parse(ipAddress);

                await _client.ConnectAsync(parsedAddress, port);

                Console.WriteLine($"Connected to {ipAddress}:{port}");

                HandleReadData(_client);
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private async Task HandleReadData(TcpClient client)
        {
            try
            {
                var streamReader = new StreamReader(client.GetStream());
                var buffer = new char[64];

                while (true)
                {
                    var byteCount = await streamReader.ReadAsync(buffer, 0, buffer.Length);

                    if (byteCount <= 0)
                    {
                        Console.WriteLine($"Connection Severed");
                        _client.Close();
                        break;
                    }

                    var message = new string(buffer);

                    Console.WriteLine(message);

                    Array.Clear(buffer, 0, buffer.Length);
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
