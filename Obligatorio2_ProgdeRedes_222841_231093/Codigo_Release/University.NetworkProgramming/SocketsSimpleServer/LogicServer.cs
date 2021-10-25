﻿using Bussiness;
using Bussiness.Domain;
using Files;
using Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketsSimpleServer
{
    public static class LogicServer
    {
        private static List<Client> clients = new List<Client>();
        public async static Task ClientHandlerAsync(NetworkStream networkStream)
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
                            case ProtocolMethods.Create:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                response = Logic.Add(request);
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, response, ProtocolMethods.Response);
                                break;
                            case ProtocolMethods.Update:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                response = Logic.Update(request);
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, response, ProtocolMethods.Response);
                                break;
                            case ProtocolMethods.Buy:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                response = Logic.Buy(request, client.boughtGames);
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, response, ProtocolMethods.Response);
                                break;
                            case ProtocolMethods.Evaluate:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                response = Logic.Evaluate(request);
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, response, ProtocolMethods.Response);
                                break;
                            case ProtocolMethods.Search:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                response = Logic.Search(request);
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, response, ProtocolMethods.Response);
                                break;
                            case ProtocolMethods.Show:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                response = Logic.Show(request);
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, response, ProtocolMethods.Response);
                                break;
                            case ProtocolMethods.ShowAll:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                response = Logic.GetAll();
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, response, ProtocolMethods.Response);
                                break;
                            case ProtocolMethods.Reviews:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                response = Logic.GetReviews(request);
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, response, ProtocolMethods.Response);
                                break;
                            case ProtocolMethods.Delete:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                response = Logic.Delete(request, clients);
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, response, ProtocolMethods.Response);
                                break;
                            case ProtocolMethods.Exit:
                                request = "Exit";
                                if (!(client is null))
                                {
                                    client.active = false;
                                }
                                break;
                            case ProtocolMethods.SendImage:
                                FileStreamHandler fileStreamHandler = new FileStreamHandler();
                                await Protocol.Protocol.ReceiveFileAsync(networkStream, header, fileStreamHandler, true);
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, "Se agrego caratula para el juego", ProtocolMethods.Response);
                                break;
                            case ProtocolMethods.UpdateRoute:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                Logic.UpdateRoute(request);
                                break;
                            case ProtocolMethods.ReceiveImage:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                var route = Logic.GetRouteImage(request);
                                FileHandler fileHandler = new FileHandler();
                                FileStreamHandler _fileStreamHandler = new FileStreamHandler();
                                await Protocol.Protocol.SendFileAsync(networkStream, route, fileHandler, ProtocolMethods.Success, request, _fileStreamHandler);
                                break;
                            case ProtocolMethods.ListBoughtGames:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                response = Logic.GetListBought(request, client.boughtGames);
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, response, ProtocolMethods.Response);
                                break;
                            case ProtocolMethods.Register:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                client = Logic.Register(request, clients);
                                Logic.ActiveUser(client);
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, "Se creo un nuevo usuario", ProtocolMethods.Response);
                                break;
                            case ProtocolMethods.Login:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                client = Logic.Login(request, clients);
                                Logic.ActiveUser(client);
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, "Se loggeo un nuevo usuario", ProtocolMethods.Response);
                                break;
                            case ProtocolMethods.Logout:
                                request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                                Logic.ActiveUser(client);
                                await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, "Hasta pronto!", ProtocolMethods.Response);
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

        public static string ShowAllUsers()
        {
            string response = "";
            int clientsNumber = 1;
            if(clients.Count == 0)
            {
                response = "No existen usuarios registrados en el sistema.";
            }
            foreach(Client client in clients)
            {
                response = response + clientsNumber + ". Nombre: " + client.name + "\n";
                clientsNumber++;
            }
            return response;
        }

        public static string DeleteUser(string name, string password)
        {
            string request = name + "-" + password;
            try
            {
                return Logic.DeleteUser(request, clients);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string UpdateUserAsync(string oldName, string oldPassword, string newName, string newPassword)
        {
            string request = oldName + "-" + oldPassword + "-" + newName + "-" + newPassword;
            try
            {
                return Logic.UpdateUser(request, clients);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string  CreateNewUser(string name, string password)
        {
            string request = name + "-" + password;
            try
            {
                Client client = Logic.Register(request, clients);
                return "Se ha creado un nuevo usuario, " + name;
            }catch(Exception e)
            {
                return e.Message;
            }
        }
    }
}
