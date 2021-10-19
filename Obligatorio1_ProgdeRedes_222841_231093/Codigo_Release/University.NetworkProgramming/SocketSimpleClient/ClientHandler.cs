using Microsoft.Extensions.Configuration;
using Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketSimpleClient
{
    public class ClientHandler
    {
        public async Task StartClientAsync()
        {
            IPEndPoint clientEndPoint = new IPEndPoint(
                IPAddress.Parse(GetIPAddressClient()),
                GetPortClient());
            TcpClient tcpClient = new TcpClient(clientEndPoint);
            Console.WriteLine("Trying to connect to server...");
            IPEndPoint serverEndPoint = new IPEndPoint(
                IPAddress.Parse(GetIPAddressServer()),
                GetPortServer());
            if (Directory.Exists(Directory.GetCurrentDirectory() + @"\CaratulasClient"))
            {
                Directory.Delete("CaratulasClient", true);

            }
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\CaratulasClient");
            await tcpClient.ConnectAsync(IPAddress.Parse(GetIPAddressServer()), GetPortServer());
            Console.WriteLine("Connected to server");
            NetworkStream networkStream = tcpClient.GetStream();
            await LogicClient.WriteServerAsync(tcpClient, networkStream);
        }

        private int GetPortServer()
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false).Build();

            int port = int.Parse(config.GetSection("portServer").Value.ToString());
            return port;
        }

        private int GetPortClient()
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false).Build();

            int port = int.Parse(config.GetSection("portClient").Value.ToString());
            return port;
        }

        private string GetIPAddressClient()
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
            return config.GetSection("ipAddressClient").Value.ToString();
        }
        private string GetIPAddressServer()
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
            return config.GetSection("ipAddressServer").Value.ToString();
        }

    }
}
