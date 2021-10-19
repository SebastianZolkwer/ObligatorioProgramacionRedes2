using Bussiness;
using Microsoft.Extensions.Configuration;
using SocketsSimpleServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class ServerHandler
    {
        private bool runServer = true;
        public ServerHandler()
        {
            IPEndPoint serverIpEndPoint = new IPEndPoint(
               IPAddress.Parse(GetIPAddress()),
               GetPort());
            TcpListener _tcpListener = new TcpListener(serverIpEndPoint);
            _tcpListener.Start(GetBackLog());
            Console.WriteLine("Start listening for client");
            if (Directory.Exists(Directory.GetCurrentDirectory() + @"\CaratulasServer"))
            {
                Directory.Delete("CaratulasServer", true);

            }
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\CaratulasServer");
            Task acceptClients = Task.Run(() => AcceptClients(_tcpListener));            
            string message = "";
            while (message != "2")
            {
                Console.WriteLine("Seleccione una opcion:\n 1. Ver Catalogo\n 2. Exit");
                message = Console.ReadLine();

                if (message == "1")
                {
                    try
                    {
                        Console.WriteLine(Logic.GetAll());
                    }catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    
                }
                else if (message != "2")
                {
                    Console.WriteLine("No fue seleccionada ninguna funcionalidad");
                }
            }
            runServer = false;
        }

        private int GetPort()
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

            int port = int.Parse(config.GetSection("port").Value.ToString());
            return port;
        }

        private int GetBackLog()
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

            int port = int.Parse(config.GetSection("backLog").Value.ToString());
            return port;
        }

        private string GetIPAddress()
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false).Build();
            return config.GetSection("ipAddress").Value.ToString();
        }
        private void AcceptClients(TcpListener tcpListener)
        {
            while (runServer)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                NetworkStream _networkStream = tcpClient.GetStream();
                Task task = Task.Run(() => LogicServer.ClientHandlerAsync(tcpClient, _networkStream));
            }
        }
    }
}
     