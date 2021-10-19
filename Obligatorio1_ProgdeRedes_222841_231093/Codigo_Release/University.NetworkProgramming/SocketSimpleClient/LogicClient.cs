using Files;
using Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketSimpleClient
{
    public static class LogicClient
    {
        public static FileHandler fileHandler = new FileHandler();
        public static  FileStreamHandler fileStreamHandler = new FileStreamHandler();
        private static bool connected = true;
        private static bool logged = false;
        public async static Task WriteServerAsync(TcpClient tcpClient, NetworkStream networkStream)
        {

            Header header = new Header();
            try
            {
                await ManageClientAsync(tcpClient, networkStream, header);
                await ActionMenuAsync(tcpClient, networkStream, header);
            }
            catch (IOException)
            {
                Console.WriteLine("Se cerro la conexion del servidor");
            }
            networkStream.Close();
            tcpClient.Close();
        }

        private static async Task ExitAsync(TcpClient tcpClient, NetworkStream networkStream)
        {
            try
            {
                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Exit, "Exit", ProtocolMethods.Request);
            }
            catch (IOException)
            { }
            networkStream.Close();
            tcpClient.Close();
        }

        private async static Task ActionMenuAsync(TcpClient tcpClient, NetworkStream networkStream, Header header)
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
                        await CreateAsync(networkStream, header);
                        break;
                    case 2:
                        await UpdateAsync(networkStream, header);
                        break;
                    case 3:
                        await BuyAsync(networkStream, header);
                        break;
                    case 4:
                        await EvaluateAsync(networkStream, header);
                        break;
                    case 5:
                        await SearchAsync(networkStream, header);
                        break;
                    case 6:
                        await ShowAsync(networkStream, header);
                        break;
                    case 7:
                        await ShowAllAsync(networkStream, header);
                        break;
                    case 8:
                        await ReviewsAsync(networkStream, header);
                        break;
                    case 9:
                        await DeleteAsync(networkStream, header);
                        break;
                    case 10:
                        connected = false;
                        await ExitAsync(tcpClient, networkStream);
                        break;
                    case 11:
                        await ListBoughtGamesAsync(networkStream, header);
                        break;
                    case 12:
                        logged = false;
                        await WriteServerAsync(tcpClient, networkStream);
                        break;
                    default:
                        Console.WriteLine("Opcion invalida");
                        break;
                }
            }
        }

        private async static Task ManageClientAsync(TcpClient tcpClient, NetworkStream networkStream, Header header)
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
                        await RegisterAsync(networkStream, header);
                        break;
                    case 2:
                        await LoginAsync (networkStream, header);
                        break;
                    case 3:
                        logged = true;
                        connected = false;
                        await ExitAsync(tcpClient, networkStream);
                        break;
                    default:
                        Console.WriteLine("Opcion invalida");
                        await WriteServerAsync(tcpClient, networkStream);
                        break;
                }
            }
        }

        private static async Task LoginAsync(NetworkStream networkStream, Header header)
        {
            string name;
            string password;

            Console.WriteLine("Ingrese Nombre de usuario");
            name = Console.ReadLine();
            Console.WriteLine("Ingrese su Password");
            password = Console.ReadLine();
            try
            {
                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Login, $"{name}-{password}", ProtocolMethods.Request);
                header = await Protocol.Protocol.ReceiveAndDecodeFixDataAsync(networkStream, header);
                string response = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
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

        private async static Task RegisterAsync(NetworkStream networkStream, Header header)
        {
            string name;
            string password;
   
            Console.WriteLine("Ingrese Nombre de usuario");
            name = Console.ReadLine();
            Console.WriteLine("Ingrese su Password");
            password = Console.ReadLine();
            try
            {
                await Protocol .Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Register, $"{name}-{password}", ProtocolMethods.Request);
                header = await Protocol.Protocol.ReceiveAndDecodeFixDataAsync(networkStream, header);
                string response = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
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

        private async static Task ListBoughtGamesAsync(NetworkStream networkStream, Header header)
        {
            try
            {
                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.ListBoughtGames, "", ProtocolMethods.Request);
                header = await Protocol.Protocol.ReceiveAndDecodeFixDataAsync(networkStream, header);
                string response = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                Console.WriteLine(response);
            }
            catch (SocketException)
            {
                connected = false;
            }
        }

        private async static Task DeleteAsync(NetworkStream networkStream, Header header)
        {
            string titleGame = "";


            Console.WriteLine("Ingrese titulo de juego que quiere borrar");
            titleGame = Console.ReadLine();
            try
            {
                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Delete, $"{titleGame}", ProtocolMethods.Request);
                header = await Protocol.Protocol.ReceiveAndDecodeFixDataAsync(networkStream, header);
                string response = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                Console.WriteLine(response);
            }
            catch (SocketException)
            {
                connected = false;
            }
            
        }

        private async static Task ReviewsAsync(NetworkStream networkStream, Header header)
        {
            string titleGame = "";


            Console.WriteLine("Ingrese titulo de juego que quiere observar las calificaciones");
            titleGame = Console.ReadLine();
            try
            {
                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Reviews, $"{titleGame}", ProtocolMethods.Request);
                header = await Protocol.Protocol.ReceiveAndDecodeFixDataAsync(networkStream, header);
                string response = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                Console.WriteLine(response);
            }
            catch (SocketException)
            {
                connected = false;
            }
            
        }

        private async static Task ShowAllAsync(NetworkStream networkStream, Header header)
        {
            try
            {
                await  Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.ShowAll, "", ProtocolMethods.Request);
                header = await Protocol.Protocol.ReceiveAndDecodeFixDataAsync(networkStream, header);
                string response = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                Console.WriteLine(response);
            }
            catch (SocketException)
            {
                connected = false;
            }
           
        }

        private async static Task  ShowAsync(NetworkStream networkStream, Header header)
        {
            string titleGame = "";


            Console.WriteLine("Ingrese titulo de juego que quiere observar detalles");
            titleGame = Console.ReadLine();
            try
            {
                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Show, $"{titleGame}", ProtocolMethods.Request);
                header = await Protocol.Protocol.ReceiveAndDecodeFixDataAsync(networkStream, header);
                string response = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
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
                        await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.ReceiveImage, titleGame, ProtocolMethods.Request);
                        header = await Protocol.Protocol.ReceiveAndDecodeFixDataAsync(networkStream, header);
                        await Protocol.Protocol.ReceiveFileAsync(networkStream, header, fileStreamHandler, false);
                        Console.WriteLine("Se ha descargado la imagen correspondiente!");
                    } catch (SocketException)
                    {
                        connected = false;
                    }
                   
                }
            }
            
        }
        private async static Task SearchAsync(NetworkStream networkStream, Header header)
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
                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Search, $"{titleGame}-{rate}-{gender}", ProtocolMethods.Request);
                header = await Protocol.Protocol.ReceiveAndDecodeFixDataAsync(networkStream, header);
                string response = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                Console.WriteLine(response);
            }
            catch (SocketException)
            {
                connected = false;
            }
            
        }

        private async static Task EvaluateAsync(NetworkStream networkStream, Header header)
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
                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Evaluate, $"{titleGame}-{rate}-{description}", ProtocolMethods.Request);
                header = await Protocol.Protocol.ReceiveAndDecodeFixDataAsync(networkStream, header);
                string response = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                Console.WriteLine(response);
            }catch (SocketException)
            {
                connected = false;
            }
        }

        private  async static Task BuyAsync(NetworkStream networkStream, Header header)
        {
            string titleGame = "";
            Console.WriteLine("Ingrese titulo de juego que quiere comprar");
            titleGame = Console.ReadLine();
            await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Buy, $"{titleGame}", ProtocolMethods.Request);
            header = await Protocol.Protocol.ReceiveAndDecodeFixDataAsync(networkStream, header);
            string response = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
            Console.WriteLine(response);
        }

        private async static Task UpdateAsync(NetworkStream networkStream, Header header)
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
                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Update, $"{titleGame}-{title}-{gender}-{sinopsis}", ProtocolMethods.Request);
                header = await Protocol.Protocol.ReceiveAndDecodeFixDataAsync(networkStream, header);
                string response = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                Console.WriteLine(response);
            }
            catch (SocketException)
            {
                connected = false;
            }  
        }

        private async static Task CreateAsync(NetworkStream networkStream, Header header)
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
                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Create, $"{title}-{gender}-{sinopsis}", ProtocolMethods.Request);
                header = await Protocol.Protocol.ReceiveAndDecodeFixDataAsync(networkStream, header);
                string response = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
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
                    await Protocol.Protocol.SendFileAsync(networkStream, path, fileHandler, ProtocolMethods.SendImage, title, fileStreamHandler);
                    header = await Protocol.Protocol.ReceiveAndDecodeFixDataAsync(networkStream, header);
                    string response = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                    Console.WriteLine(response);
                   await  Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.UpdateRoute, $"{title}-{type}", ProtocolMethods.Request);
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