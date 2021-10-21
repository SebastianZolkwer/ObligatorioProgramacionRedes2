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
            Task.Run(() => AcceptClients(_tcpListener));            
            
            RunFuncionalitiesMenu();
            
            runServer = false;
        }

        private void  RunFuncionalitiesMenu()
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
                         ShowAllGames();
                        break;
                    case 2:
                         CreateUser();
                        break;
                    case 3:
                        UpdateUser();
                        break;
                    case 4:
                         DeleteUser();
                        break;
                    case 5:
                        ShowAllUsers();
                        break;
                    case 6:
                        break;
                    default:
                        Console.WriteLine("Opcion invalida");
                        break;
                }  
            }
        }

        private void ShowAllUsers()
        {
            Console.WriteLine( LogicServer.ShowAllUsers());
        }

        private void  CreateUser()
        {
            string name;
            string password;

            Console.WriteLine("Ingrese Nombre de usuario");
            name = Console.ReadLine();
            Console.WriteLine("Ingrese Password");
            password = Console.ReadLine();
           Console.WriteLine(LogicServer.CreateNewUser(name, password));
        }

        private void UpdateUser()
        {
            string oldName;
            string oldPassword;
            string newName;
            string newPassword;
            Console.WriteLine("Ingrese Nombre de usuario anterior");
            oldName = Console.ReadLine();
            Console.WriteLine("Ingrese Password anterior");
            oldPassword = Console.ReadLine();
            Console.WriteLine("Ingrese Nombre de usuario nuevo");
            newName = Console.ReadLine();
            Console.WriteLine("Ingrese Password nueva");
            newPassword = Console.ReadLine();
            Console.WriteLine( LogicServer.UpdateUserAsync(oldName, oldPassword, newName, newPassword));

        }

        private void DeleteUser()
        {
            string name;
            string password;

            Console.WriteLine("Ingrese Nombre de usuario");
            name = Console.ReadLine();
            Console.WriteLine("Ingrese Password");
            password = Console.ReadLine();
            Console.WriteLine(LogicServer.DeleteUser(name, password));

        }

        private void  ShowAllGames()
        {
            try
            {
                Console.WriteLine(Logic.GetAll());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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
     