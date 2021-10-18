using Files;
using Protocol;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace SocketSimpleClient
{
    public static class LogicClient
    {
        public static FileHandler fileHandler = new FileHandler();
        public static  FileStreamHandler fileStreamHandler = new FileStreamHandler();
        private static bool connected = true;
        private static bool logged = false;
        public static void WriteServer(Socket socket)
        {
            Header header = new Header();
            ManageClient(socket, header);
            ActionMenu(socket, header);
        }

        private static void Exit(Socket socket)
        {
            try
            {
                Protocol.Protocol.SendAndCode(socket, ProtocolMethods.Exit, "Exit", ProtocolMethods.Request);
            }
            catch (SocketException)
            { }
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        private static void ActionMenu(Socket socket, Header header)
        {
            while (connected)
            {
                Console.WriteLine(ShowMenu());
                int option = 0;
                try
                {
                    option = GetNumber(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                switch (option)
                {
                    case 1:
                        Create(socket, header);
                        break;
                    case 2:
                        Update(socket, header);
                        break;
                    case 3:
                        Buy(socket, header);
                        break;
                    case 4:
                        Evaluate(socket, header);
                        break;
                    case 5:
                        Search(socket, header);
                        break;
                    case 6:
                        Show(socket, header);
                        break;
                    case 7:
                        ShowAll(socket, header);
                        break;
                    case 8:
                        Reviews(socket, header);
                        break;
                    case 9:
                        Delete(socket, header);
                        break;
                    case 10:
                        connected = false;
                        Exit(socket);
                        break;
                    case 11:
                        ListBoughtGames(socket, header);
                        break;
                    case 12:
                        logged = false;
                        WriteServer(socket);
                        break;
                    default:
                        Console.WriteLine("Opcion invalida");
                        //WriteServer(socket);
                        break;
                }

            }
        }

        private static void ManageClient(Socket socket, Header header)
        {
            while (!logged)
            {
                Console.WriteLine(ShowInitialMenu());
                int login = 0;
                try
                {
                    login = GetNumber(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                switch (login)
                {
                    case 1:
                        Register(socket, header);
                        break;
                    case 2:
                        Login(socket, header);
                        break;
                    case 3:
                        logged = true;
                        connected = false;
                        Exit(socket);
                        break;
                    default:
                        Console.WriteLine("Opcion invalida");
                        WriteServer(socket);
                        break;
                }
            }
        }

        private static void Login(Socket socket, Header header)
        {
            string name;
            string password;

            Console.WriteLine("Ingrese Nombre de usuario");
            name = Console.ReadLine();
            Console.WriteLine("Ingrese su Password");
            password = Console.ReadLine();
            try
            {
                Protocol.Protocol.SendAndCode(socket, ProtocolMethods.Login, $"{name}-{password}", ProtocolMethods.Request);
                header = Protocol.Protocol.ReceiveAndDecodeFixData(socket, header);
                string response = Protocol.Protocol.RecieveAndDecodeVariableData(socket, header.GetDataLength());
                Console.WriteLine(response);
            }
            catch (SocketException)
            {
                // connected = false;
            }

            if (header.GetMethod() != ProtocolMethods.Error)
            {
                Console.WriteLine("¡" + name + ", bienvenido nuevamente al sistema!");
                logged = true;
            }
        }

        private static void Register(Socket socket, Header header)
        {
            string name;
            string password;
   
            Console.WriteLine("Ingrese Nombre de usuario");
            name = Console.ReadLine();
            Console.WriteLine("Ingrese su Password");
            password = Console.ReadLine();
            try
            {
                Protocol.Protocol.SendAndCode(socket, ProtocolMethods.Register, $"{name}-{password}", ProtocolMethods.Request);
                header = Protocol.Protocol.ReceiveAndDecodeFixData(socket, header);
                string response = Protocol.Protocol.RecieveAndDecodeVariableData(socket, header.GetDataLength());
                Console.WriteLine(response);
            }
            catch (SocketException)
            {
               // connected = false;
            }

            if (header.GetMethod() != ProtocolMethods.Error)
            {
                Console.WriteLine("¡" + name + ", bienvenido al sistema!");
                logged = true;
            }
        }

        private static string ShowInitialMenu()
        {
            return "Seleccione una opcion:\n 1. Registrarse\n 2. Iniciar Sesion\n 3. Salir";
        }

        private static void ListBoughtGames(Socket socket, Header header)
        {
            try
            {
                Protocol.Protocol.SendAndCode(socket, ProtocolMethods.ListBoughtGames, "", ProtocolMethods.Request);
                header = Protocol.Protocol.ReceiveAndDecodeFixData(socket, header);
                string response = Protocol.Protocol.RecieveAndDecodeVariableData(socket, header.GetDataLength());
                Console.WriteLine(response);
            }
            catch (SocketException)
            {
                connected = false;
            }
        }

        private static void Delete(Socket socket, Header header)
        {
            string titleGame = "";


            Console.WriteLine("Ingrese titulo de juego que quiere borrar");
            titleGame = Console.ReadLine();
            try
            {
                Protocol.Protocol.SendAndCode(socket, ProtocolMethods.Delete, $"{titleGame}", ProtocolMethods.Request);
                header = Protocol.Protocol.ReceiveAndDecodeFixData(socket, header);
                string response = Protocol.Protocol.RecieveAndDecodeVariableData(socket, header.GetDataLength());
                Console.WriteLine(response);
            }
            catch (SocketException)
            {
                connected = false;
            }
            
        }

        private static void Reviews(Socket socket, Header header)
        {
            string titleGame = "";


            Console.WriteLine("Ingrese titulo de juego que quiere observar las calificaciones");
            titleGame = Console.ReadLine();
            try
            {
                Protocol.Protocol.SendAndCode(socket, ProtocolMethods.Reviews, $"{titleGame}", ProtocolMethods.Request);
                header = Protocol.Protocol.ReceiveAndDecodeFixData(socket, header);
                string response = Protocol.Protocol.RecieveAndDecodeVariableData(socket, header.GetDataLength());
                Console.WriteLine(response);
            }
            catch (SocketException)
            {
                connected = false;
            }
            
        }

        private static void ShowAll(Socket socket, Header header)
        {
            try
            {
                Protocol.Protocol.SendAndCode(socket, ProtocolMethods.ShowAll, "", ProtocolMethods.Request);
                header = Protocol.Protocol.ReceiveAndDecodeFixData(socket, header);
                string response = Protocol.Protocol.RecieveAndDecodeVariableData(socket, header.GetDataLength());
                Console.WriteLine(response);
            }
            catch (SocketException)
            {
                connected = false;
            }
           
        }

        private static void Show(Socket socket, Header header)
        {
            string titleGame = "";


            Console.WriteLine("Ingrese titulo de juego que quiere observar detalles");
            titleGame = Console.ReadLine();
            try
            {
                Protocol.Protocol.SendAndCode(socket, ProtocolMethods.Show, $"{titleGame}", ProtocolMethods.Request);
                header = Protocol.Protocol.ReceiveAndDecodeFixData(socket, header);
                string response = Protocol.Protocol.RecieveAndDecodeVariableData(socket, header.GetDataLength());
                Console.WriteLine(response);
            }
            catch (SocketException)
            {
                connected = false;
            }
            if(header.GetMethod() == ProtocolMethods.Success)
            {
                Console.WriteLine("Desea descargar la caratula? Si/No" );
                string answer = "";
                try
                {
                    answer = Console.ReadLine().ToLower();
                }
                catch { }
                if(answer == "si")
                {
                    try
                    {
                        Protocol.Protocol.SendAndCode(socket, ProtocolMethods.ReceiveImage, titleGame, ProtocolMethods.Request);
                        header = Protocol.Protocol.ReceiveAndDecodeFixData(socket, header);
                        Protocol.Protocol.ReceiveFile(socket, header, fileStreamHandler, false);
                        Console.WriteLine("Se ha descargado la imagen correspondiente!");
                    } catch (SocketException)
                    {
                        connected = false;
                    }
                   
                }
            }
            
        }
        private static void Search(Socket socket, Header header)
        {
            string gender = "";
            string rate = "";
            string titleGame = "";

            Console.WriteLine("Ingrese titulo de juego que quiere filtrar");
            titleGame = Console.ReadLine();
            Console.WriteLine("Ingrese calificacion por la quiere filtrar");
            rate = Console.ReadLine();
            Console.WriteLine("Ingrese genero por el que quiere filtrar");
            gender = Console.ReadLine();
            try
            {
                Protocol.Protocol.SendAndCode(socket, ProtocolMethods.Search, $"{titleGame}-{rate}-{gender}", ProtocolMethods.Request);
                header = Protocol.Protocol.ReceiveAndDecodeFixData(socket, header);
                string response = Protocol.Protocol.RecieveAndDecodeVariableData(socket, header.GetDataLength());
                Console.WriteLine(response);
            }
            catch (SocketException)
            {
                connected = false;
            }
            
        }

        private static void Evaluate(Socket socket, Header header)
        {
            string rate;
            string description;
            string titleGame = "";
            

            Console.WriteLine("Ingrese titulo de juego que quiere calificar");
            titleGame = Console.ReadLine();
            Console.WriteLine("Ingrese calificacion de juego");
            rate = Console.ReadLine();
            Console.WriteLine("Ingrese descripcion");
            description = Console.ReadLine();
            try
            {
                Protocol.Protocol.SendAndCode(socket, ProtocolMethods.Evaluate, $"{titleGame}-{rate}-{description}", ProtocolMethods.Request);
                header = Protocol.Protocol.ReceiveAndDecodeFixData(socket, header);
                string response = Protocol.Protocol.RecieveAndDecodeVariableData(socket, header.GetDataLength());
                Console.WriteLine(response);
            }catch (SocketException)
            {
                connected = false;
            }
        }

        private static void Buy(Socket socket, Header header)
        {
            string titleGame = "";
            Console.WriteLine("Ingrese titulo de juego que quiere comprar");
            titleGame = Console.ReadLine();
            Protocol.Protocol.SendAndCode(socket, ProtocolMethods.Buy, $"{titleGame}", ProtocolMethods.Request);
            header = Protocol.Protocol.ReceiveAndDecodeFixData(socket, header);
            string response = Protocol.Protocol.RecieveAndDecodeVariableData(socket, header.GetDataLength());
            Console.WriteLine(response);
        }

        private static void Update(Socket socket, Header header)
        {
            string title = "";
            string gender = "";
            string sinopsis = "";
            string titleGame = "";

            Console.WriteLine("Ingrese titulo de juego que quiere modificar");
            titleGame = Console.ReadLine();
            Console.WriteLine("Ingrese NUEVO titulo de juego");
            title = Console.ReadLine();
            Console.WriteLine("Ingrese genero");
            gender = Console.ReadLine();
            Console.WriteLine("Ingrese sinopsis");
            sinopsis = Console.ReadLine();
            try
            {
                Protocol.Protocol.SendAndCode(socket, ProtocolMethods.Update, $"{titleGame}-{title}-{gender}-{sinopsis}", ProtocolMethods.Request);
                header = Protocol.Protocol.ReceiveAndDecodeFixData(socket, header);
                string response = Protocol.Protocol.RecieveAndDecodeVariableData(socket, header.GetDataLength());
                Console.WriteLine(response);
            }
            catch (SocketException)
            {
                connected = false;
            }  
        }

        private static void Create(Socket socket, Header header)
        {
            string title;
            string gender;
            string sinopsis;
            string type = ".";

            Console.WriteLine("Ingrese titulo de juego");
            title = Console.ReadLine();
            Console.WriteLine("Ingrese genero");
            gender = Console.ReadLine();
            Console.WriteLine("Ingrese sinopsis");
            sinopsis = Console.ReadLine();
            try
            {
                Protocol.Protocol.SendAndCode(socket, ProtocolMethods.Create, $"{title}-{gender}-{sinopsis}", ProtocolMethods.Request);
                header = Protocol.Protocol.ReceiveAndDecodeFixData(socket, header);
                string response = Protocol.Protocol.RecieveAndDecodeVariableData(socket, header.GetDataLength());
                Console.WriteLine(response);
            }
            catch (SocketException)
            {
                connected = false;
            }

            if (header.GetMethod() != ProtocolMethods.Error)
            {
                string path = string.Empty;
                
                while (path == null ||  path.Equals(string.Empty) || !fileHandler.FileExists(path))
                {
                    Console.WriteLine("Ingrese la ruta de la caratula");
                    path = Console.ReadLine();
                    var nameFile = path.Split('.');
                    type = type + nameFile[nameFile.Length - 1];
                }
                try
                {
                    Protocol.Protocol.SendFile(socket, path, fileHandler, ProtocolMethods.SendImage, title, fileStreamHandler);
                    header = Protocol.Protocol.ReceiveAndDecodeFixData(socket, header);
                    string response = Protocol.Protocol.RecieveAndDecodeVariableData(socket, header.GetDataLength());
                    Console.WriteLine(response);
                    Protocol.Protocol.SendAndCode(socket, ProtocolMethods.UpdateRoute, $"{title}-{type}", ProtocolMethods.Request);
                }
                catch (SocketException) {
                    connected = false;
                }
                

            }
        }

        private static string ShowMenu()
        {
            return "Seleccione una opcion:\n 1. Crear Juego\n 2. Modificar Juego\n 3. Comprar Juego\n 4. Calificar Juego\n 5. Buscar Juego\n 6. Ver Juego\n 7. Ver Catalogo\n 8. Ver Reviews de un Juego\n 9. Eliminar juego\n 10. Exit\n 11. Obtener lista de comprados\n 12. Logout";
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