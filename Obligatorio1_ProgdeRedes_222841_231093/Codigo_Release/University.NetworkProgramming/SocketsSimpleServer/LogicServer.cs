using Bussiness;
using Bussiness.Domain;
using Files;
using Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketsSimpleServer
{
    public static class LogicServer
    {
        private static List<Client> clients = new List<Client>();
        public async static Task ClientHandlerAsync(TcpClient tcpClient, NetworkStream networkStream)
        {
            {
                string request = "";
                Client client = null;
                while (request != "Exit")
                {
                    try
                    {
                        var header = new Header();
                        header = await Protocol.Protocol.ReceiveAndDecodeFixDataAsync(networkStream, header);
                        string response;
                        switch (header.GetMethod())
                        {
                            case 1:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                response = Logic.Add(request);
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, response, ProtocolMethods.Response);
                                break;
                            case 2:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                response = Logic.Update(request);
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, response, ProtocolMethods.Response);
                                break;
                            case 3:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                response = Logic.Buy(request, client.boughtGames);
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, response, ProtocolMethods.Response);
                                break;
                            case 4:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                response = Logic.Evaluate(request);
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, response, ProtocolMethods.Response);
                                break;
                            case 5:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                response = Logic.Search(request);
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, response, ProtocolMethods.Response);
                                break;
                            case 6:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                response = Logic.Show(request);
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, response, ProtocolMethods.Response);
                                break;
                            case 7:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                response = Logic.GetAll();
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, response, ProtocolMethods.Response);
                                break;
                            case 8:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                response = Logic.GetReviews(request);
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, response, ProtocolMethods.Response);
                                break;
                            case 9:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                response = Logic.Delete(request, clients);
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, response, ProtocolMethods.Response);
                                break;
                            case 10:
                                request = "Exit";
                                break;
                            case 11:
                                FileStreamHandler fileStreamHandler = new FileStreamHandler();
                                await Protocol.Protocol.ReceiveFileAsync(networkStream, header, fileStreamHandler, true);
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, "Se agrego caratula para el juego", ProtocolMethods.Response);
                                break;
                            case 12:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                Logic.UpdateRoute(request);
                                break;
                            case 13:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                var route = Logic.GetRouteImage(request);
                                FileHandler fileHandler = new FileHandler();
                                FileStreamHandler _fileStreamHandler = new FileStreamHandler();
                                await Protocol.Protocol.SendFileAsync(networkStream, route, fileHandler, 1, request, _fileStreamHandler);
                                break;
                            case 14:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                response = Logic.GetListBought(request, client.boughtGames);
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, response, ProtocolMethods.Response);
                                break;
                            case 15:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                client = Logic.Register(request, clients);
                                clients.Add(client);
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, "Se creo un nuevo usuario", ProtocolMethods.Response);
                                break;
                            case 16:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                client = Logic.Login(request, clients);
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, "Se loggeo un nuevo usuario", ProtocolMethods.Response);
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        await Protocol.Protocol.SendAndCodeAsync(networkStream, 0, e.Message, ProtocolMethods.Response);
                    }
                }
            }
        }

        internal static void DeleteUserAsync(string name, string password)
        {
            string request = name + "-" + password;
            try
            {
                Client client = Logic.DeleteUser(request, clients);
                clients.Remove(client);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        internal static void UpdateUserAsync(string oldName, string oldPassword, string newName, string newPassword)
        {
            string request = oldName + "-" + oldPassword + "-" + newName + "-" + newPassword;
            try
            {
                Client client = Logic.UpdateUser(request, clients);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void CreateNewUserAsync(string name, string password)
        {
            string request = name + "-" + password;
            try
            {
                Client client = Logic.Register(request, clients);
                clients.Add(client);
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
