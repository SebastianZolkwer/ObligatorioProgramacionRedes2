using Bussiness;
using Bussiness.Domain;
using Files;
using Grpc.Net.Client;
using Protocol;
using ServerAdmin;
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
        private static GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:5001");
        private static Greeter.GreeterClient user = new Greeter.GreeterClient(channel);
        public async static Task ClientHandlerAsync(NetworkStream networkStream)
        {
            {
                string request = "";
                string client = null;

                while (request != "Exit")
                {

                    var header = new Header();
                    header = await Protocol.Protocol.ReceiveAndDecodeFixDataAsync(networkStream, header);
                    Response response;
                    ResponseClient responseClient;
                    GameResponse gameResponse;
                    switch (header.GetMethod())
                    {
                        case ProtocolMethods.Create:
                            request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                            gameResponse = await user.CreateGameAsync(new Request
                            {
                                Attributes = request,
                                Name = client
                            });
                            await Protocol.Protocol.SendAndCodeAsync(networkStream, gameResponse.Status, gameResponse.Message, ProtocolMethods.Response);
                            break;
                        case ProtocolMethods.Update:
                            request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                            gameResponse = await user.UpdateGameAsync(new Request
                            {
                                Attributes = request,
                                Name = client
                            });
                            await Protocol.Protocol.SendAndCodeAsync(networkStream, gameResponse.Status, gameResponse.Message, ProtocolMethods.Response);
                            break;
                        case ProtocolMethods.Buy:
                            request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                            response = await user.BuyGameAsync(new Request
                            {
                                Attributes = request,
                                Name = client
                            });
                            await Protocol.Protocol.SendAndCodeAsync(networkStream, response.Status, response.Message, ProtocolMethods.Response);
                            break;
                        case ProtocolMethods.Evaluate:
                            request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                            response = await user.EvaluateGameAsync(new Request
                            {
                                Attributes = request,
                                Name = client
                            });
                            await Protocol.Protocol.SendAndCodeAsync(networkStream, response.Status, response.Message, ProtocolMethods.Response);
                            break;
                        case ProtocolMethods.Search:
                            request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                            response = await user.SearchAsync(new Request
                            {
                                Attributes = request,
                                Name = client
                            });
                            await Protocol.Protocol.SendAndCodeAsync(networkStream, response.Status, response.Message, ProtocolMethods.Response);
                            break;
                        case ProtocolMethods.Show:
                            request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                            response = await user.ShowAsync(new Request
                            {
                                Attributes = request,
                                Name = client
                            });
                            await Protocol.Protocol.SendAndCodeAsync(networkStream, response.Status, response.Message, ProtocolMethods.Response);
                            break;
                        case ProtocolMethods.ShowAll:
                            request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                            response = await user.ShowAllGamesAsync(new Request
                            {
                                Attributes = request,
                                Name = client
                            });
                            await Protocol.Protocol.SendAndCodeAsync(networkStream, response.Status, response.Message, ProtocolMethods.Response);
                            break;
                        case ProtocolMethods.Reviews:
                            request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                            response = await user.ReviewsGameAsync(new Request
                            {
                                Attributes = request,
                                Name = client
                            });
                            await Protocol.Protocol.SendAndCodeAsync(networkStream, response.Status, response.Message, ProtocolMethods.Response);
                            break;
                        case ProtocolMethods.Delete:
                            request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                            response = await user.DeleteGameAsync(new Request
                            {
                                Attributes = request,
                                Name = client
                            });
                            await Protocol.Protocol.SendAndCodeAsync(networkStream, response.Status, response.Message, ProtocolMethods.Response);
                            break;
                        case ProtocolMethods.Exit:
                            request = "Exit";
                            response = await user.LogOutAsync(new Request
                            {
                                Attributes = client,
                                Name = client
                            });
                            break;
                        case ProtocolMethods.SendImage:
                            FileStreamHandler fileStreamHandler = new FileStreamHandler();
                            await Protocol.Protocol.ReceiveFileAsync(networkStream, header, fileStreamHandler, true);
                            await Protocol.Protocol.SendAndCodeAsync(networkStream, ProtocolMethods.Success, "Se agrego caratula para el juego", ProtocolMethods.Response);
                            break;
                        case ProtocolMethods.UpdateRoute:
                            request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                            response = await user.UpdateRouteAsync(new Request
                            {
                                Attributes = request,
                                Name = client
                            });

                            break;
                        case ProtocolMethods.ReceiveImage:
                            request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                            response = await user.GetRouteImageAsync(new Request
                            {
                                Attributes = request,
                                Name = client
                            });
                            FileHandler fileHandler = new FileHandler();
                            FileStreamHandler _fileStreamHandler = new FileStreamHandler();
                            await Protocol.Protocol.SendFileAsync(networkStream, response.Message, fileHandler, ProtocolMethods.Success, request, _fileStreamHandler);
                            break;
                        case ProtocolMethods.ListBoughtGames:
                            request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                            response = await user.BoughtGamesAsync(new Request
                            {
                                Name = client,
                                Attributes = client
                            });
                            await Protocol.Protocol.SendAndCodeAsync(networkStream, response.Status, response.Message, ProtocolMethods.Response);
                            break;
                        case ProtocolMethods.Register:
                            request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                            responseClient = await user.RegisterAsync(new Request
                            {
                                Attributes = request
                            });
                            client = responseClient.Name;
                            await Protocol.Protocol.SendAndCodeAsync(networkStream, responseClient.Status, responseClient.Message, ProtocolMethods.Response);
                            break;
                        case ProtocolMethods.Login:
                            request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                            responseClient = await user.LoginAsync(new Request
                            {
                                Attributes = request
                            });
                            client = responseClient.Name;
                            await Protocol.Protocol.SendAndCodeAsync(networkStream, responseClient.Status, responseClient.Message, ProtocolMethods.Response);
                            break;
                        case ProtocolMethods.Logout:
                            request = await Protocol.Protocol.RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
                            response = await user.LogOutAsync(new Request
                            {
                                Name = client
                            });
                            await Protocol.Protocol.SendAndCodeAsync(networkStream, response.Status, response.Message, ProtocolMethods.Response);
                            break;
                    }
                }
            }
        }

        public static async Task<string> ShowAllGamesAsync()
        {
            Response response = await user.ShowAllGamesAsync(new Request
            {
                Attributes = "",
                Name = "Server"
            });
            return response.Message;
        }

        public static async Task<string> ShowAllUsersAsync()
        {
            Response response = await user.ShowAllUsersAsync(new Request
            {
                Attributes = "",
                Name = "Server"
            });
            return response.Message;
        }

        public static async Task<string> DeleteUserAsync(string name)

        {
            Response response = await user.DeleteUserAsync(new Request
            {
                Attributes = name,
                Name = "Server"
            });
            return response.Message;
        }

        public static async Task<string> UpdateUserAsync(string oldName, string newName, string newPassword)
        {
            string request = oldName + "-" + newName + "-" + newPassword;
            ResponseClient response = await user.UpdateUserAsync(new Request
            {
                Attributes = request,
                Name = "Server"
            });
            return response.Message;
        }

        public static async Task<string> CreateNewUserAsync(string name, string password)
        {
            string request = name + "-" + password;
            ResponseClient responseClient = await user.RegisterAdminOrServerAsync(new Request
            {
                Attributes = request,
                Name = "Server"
            });
            return responseClient.Message + " " + name;
        }
    }
}
