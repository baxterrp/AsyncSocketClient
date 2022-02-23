using System.Net;

namespace AsyncSocketClient
{
    public class ServerConfiguration
    {
        public string IpAddress { get; set; }
        public int Port { get; set; }

        public IPAddress GetIPAddress => IPAddress.Parse(IpAddress);
    }
}
