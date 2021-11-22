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
        public async Task StartServer()
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
            _ = Task.Run(() => AcceptClients(_tcpListener));

            await Task.Run(() => RunFuncionalitiesMenuAsync());

            runServer = false;
        }

        private async Task RunFuncionalitiesMenuAsync()
        {
            int message = -1;
            while (message != 6)
            {
                WriteMenu();
                try
                {
                    message = GetNumber(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    message = -1;
                }
                switch (message)
                {
                    case 1:
                        await ShowAllGamesAsync();
                        break;
                    case 2:
                        await CreateUserAsync();
                        break;
                    case 3:
                        await UpdateUserAsync();
                        break;
                    case 4:
                        await DeleteUserAsync();
                        break;
                    case 5:
                        await ShowAllUsersAsync();
                        break;
                    case 6:
                        break;
                    default:
                        Console.WriteLine("Opcion invalida");
                        break;
                }
            }
        }

        private async Task ShowAllUsersAsync()
        {
            Console.WriteLine(await LogicServer.ShowAllUsersAsync());
        }

        private async Task CreateUserAsync()
        {
            string name;
            string password;

            Console.WriteLine("Ingrese Nombre de usuario");
            name = Console.ReadLine();
            Console.WriteLine("Ingrese Password");
            password = Console.ReadLine();
            Console.WriteLine(await LogicServer.CreateNewUserAsync(name, password));
        }

        private async Task UpdateUserAsync()
        {
            string oldName;
            string newName;
            string newPassword;
            Console.WriteLine("Ingrese Nombre de usuario anterior");
            oldName = Console.ReadLine();
            Console.WriteLine("Ingrese Nombre de usuario nuevo");
            newName = Console.ReadLine();
            Console.WriteLine("Ingrese Password nueva");
            newPassword = Console.ReadLine();
            Console.WriteLine(await LogicServer.UpdateUserAsync(oldName, newName, newPassword));

        }

        private async Task DeleteUserAsync()
        {
            string name;
            Console.WriteLine("Ingrese Nombre de usuario");
            name = Console.ReadLine();
            Console.WriteLine(await LogicServer.DeleteUserAsync(name));
        }

        private async Task ShowAllGamesAsync()
        {
            Console.WriteLine(await LogicServer.ShowAllGamesAsync());
        }

        private void WriteMenu()
        {
            Console.WriteLine("Seleccione una opcion:\n 1. Ver Catalogo de Juegos\n 2. Crear Usuario\n 3. Editar Usuario\n 4. Eliminar Usuario\n 5. Ver Lista de Usuarios\n 6. Exit");
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
                Task.Run(async () => await LogicServer.ClientHandlerAsync(_networkStream));
            }
        }
        private static int GetNumber(string number)
        {
            try
            {
                int rating = Int32.Parse(number);
                return rating;
            }
            catch
            {
                throw new Exception("La opcion elegida debe ser un numero");
            }
        }
    }
}
