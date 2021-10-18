using Bussiness;
using Bussiness.Domain;
using Files;
using Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace SocketsSimpleServer
{
    public static class LogicServer
    {
        private static List<Client> clients = new List<Client>();
        public static void ClientHandler( Socket clientSocket)
        {
            string request = "";
            Client client = null;
            while (request != "Exit")
            {
                try
                {
                    var header = new Header();
                    header = Protocol.Protocol.ReceiveAndDecodeFixData(clientSocket, header);
                    string response;
                    switch (header.GetMethod())
                    {
                        case 15:
                            request = Protocol.Protocol.RecieveAndDecodeVariableData(clientSocket, header.GetDataLength());
                            client = Logic.Register(request, clients);
                            clients.Add(client);
                            Protocol.Protocol.SendAndCode(clientSocket, ProtocolMethods.Success, "Se creo un nuevo usuario", ProtocolMethods.Response);
                            break;
                        case 16:
                            request = Protocol.Protocol.RecieveAndDecodeVariableData(clientSocket, header.GetDataLength());
                            client = Logic.Login(request, clients);
                            Protocol.Protocol.SendAndCode(clientSocket, ProtocolMethods.Success, "Se loggeo un nuevo usuario", ProtocolMethods.Response);
                            break;

                        case 1:
                            request = Protocol.Protocol.RecieveAndDecodeVariableData(clientSocket, header.GetDataLength());
                            response = Logic.Add(request);
                            Protocol.Protocol.SendAndCode(clientSocket, ProtocolMethods.Success, response, ProtocolMethods.Response);
                            break;
                        case 2:
                            request = Protocol.Protocol.RecieveAndDecodeVariableData(clientSocket, header.GetDataLength());
                            response = Logic.Update(request);
                            Protocol.Protocol.SendAndCode(clientSocket, ProtocolMethods.Success, response, ProtocolMethods.Response);
                            break;
                        case 3:
                            request = Protocol.Protocol.RecieveAndDecodeVariableData(clientSocket, header.GetDataLength());
                            response = Logic.Buy(request, client.boughtGames);
                            Protocol.Protocol.SendAndCode(clientSocket, ProtocolMethods.Success, response, ProtocolMethods.Response);
                            break;
                        case 4:
                            request = Protocol.Protocol.RecieveAndDecodeVariableData(clientSocket, header.GetDataLength());
                            response = Logic.Evaluate(request);
                            Protocol.Protocol.SendAndCode(clientSocket, ProtocolMethods.Success, response, ProtocolMethods.Response);
                            break;
                        case 5:
                            request = Protocol.Protocol.RecieveAndDecodeVariableData(clientSocket, header.GetDataLength());
                            response = Logic.Search(request);
                            Protocol.Protocol.SendAndCode(clientSocket, ProtocolMethods.Success, response, ProtocolMethods.Response);
                            break;
                        case 6:
                            request = Protocol.Protocol.RecieveAndDecodeVariableData(clientSocket, header.GetDataLength());
                            response = Logic.Show(request);
                            Protocol.Protocol.SendAndCode(clientSocket, ProtocolMethods.Success, response, ProtocolMethods.Response);
                            break;
                        case 7:
                            request = Protocol.Protocol.RecieveAndDecodeVariableData(clientSocket, header.GetDataLength());
                            response = Logic.GetAll();
                            Protocol.Protocol.SendAndCode(clientSocket, ProtocolMethods.Success, response, ProtocolMethods.Response);
                            break;
                        case 8:
                            request = Protocol.Protocol.RecieveAndDecodeVariableData(clientSocket, header.GetDataLength());
                            response = Logic.GetReviews(request);
                            Protocol.Protocol.SendAndCode(clientSocket, ProtocolMethods.Success, response, ProtocolMethods.Response);
                            break;
                        case 9:
                            request = Protocol.Protocol.RecieveAndDecodeVariableData(clientSocket, header.GetDataLength());
                            response = Logic.Delete(request, clients);
                            Protocol.Protocol.SendAndCode(clientSocket, ProtocolMethods.Success, response, ProtocolMethods.Response);
                            break;
                        case 10:
                            request = "Exit";
                            break;
                        case 11:
                            FileStreamHandler fileStreamHandler = new FileStreamHandler();
                            Protocol.Protocol.ReceiveFile(clientSocket, header, fileStreamHandler, true);
                            Protocol.Protocol.SendAndCode(clientSocket, ProtocolMethods.Success, "Se agrego caratula para el juego", ProtocolMethods.Response);
                            break;
                        case 12:
                            request = Protocol.Protocol.RecieveAndDecodeVariableData(clientSocket, header.GetDataLength());
                            Logic.UpdateRoute(request);
                            break;
                        case 13:
                            request = Protocol.Protocol.RecieveAndDecodeVariableData(clientSocket, header.GetDataLength());
                            var route = Logic.GetRouteImage(request);
                            FileHandler fileHandler = new FileHandler();                            
                            FileStreamHandler _fileStreamHandler = new FileStreamHandler();
                            Protocol.Protocol.SendFile(clientSocket, route, fileHandler, 1, request, _fileStreamHandler);
                            break;
                        case 14:
                            request = Protocol.Protocol.RecieveAndDecodeVariableData(clientSocket, header.GetDataLength());
                            response = Logic.GetListBought(request, client.boughtGames);
                            Protocol.Protocol.SendAndCode(clientSocket, ProtocolMethods.Success, response, ProtocolMethods.Response);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Protocol.Protocol.SendAndCode(clientSocket, 0, e.Message, ProtocolMethods.Response);
                }
            }
        }
    }
}
