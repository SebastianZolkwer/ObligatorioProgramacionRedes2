using Microsoft.Extensions.Configuration;
using Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SocketSimpleClient
{
    public class ClientHandler
    {
        public ClientHandler()
        {
            Socket clientSocket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
            IPEndPoint clientEndPoint = new IPEndPoint(
                IPAddress.Parse(GetIPAddressClient()),
                GetPortClient());
            clientSocket.Bind(clientEndPoint);
            Console.WriteLine("Trying to connect to server...");
            IPEndPoint serverEndPoint = new IPEndPoint(
                IPAddress.Parse(GetIPAddressServer()),
                GetPortServer());
            if (Directory.Exists(Directory.GetCurrentDirectory() + @"\CaratulasClient"))
            {
                Directory.Delete("CaratulasClient", true);

            }
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\CaratulasClient");
            clientSocket.Connect(serverEndPoint);
            Console.WriteLine("Connected to server");
            LogicClient.WriteServer(clientSocket);
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
