using BusinessLogicInterface.Interfaces;
using Bussiness.Domain;
using Grpc.Net.Client;
using Model;
using Protocol;
using ServerAdmin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Logic
{

    public class ClientLogic : IClientLogic
    {
        private static GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:5001");
        private static Greeter.GreeterClient user = new Greeter.GreeterClient(channel);

        public async Task<string> BuyGameAsync(string name, string title)
        {
            Response response = await user.AsociateGameAsync(new RequestClient
            {
                Attributes = title,
                Client = name,
                Name = "Admin"
            });
            if (response.Status == ProtocolMethods.Error)
            {
                throw new Exception(response.Message);
            }
            return response.Message;
        }


        public async Task<Client> CreateAsync(Client client)
        {
            client.Validate();
            string request =  client.Name + "-" + client.Password;
            ResponseClient responseClient = await user.RegisterAsync(new Request
            {
                Attributes = request,
                Name = "Admin"
            });
            if (responseClient.Status == ProtocolMethods.Error)
            {
                throw new Exception(responseClient.Message);
            }
            Client newClient = new Client
            {
                Name = responseClient.Name,
                Password = responseClient.Password
            };
            return newClient;
        }

        public async Task<string> DeleteAsync(string name)
        {
            Response response = await user.DeleteUserAsync(new Request
            {
                Name = "Admin",
                Attributes = name
            });
            if (response.Status == ProtocolMethods.Error)
            {
                throw new Exception(response.Message);
            }
            return response.Message;

        }


        public async Task<List<Client>> GetAllAsync()
        {
            Response response = await user.ShowAllUsersAsync(new Request
            {
                Attributes = "",
                Name = "Admin"
            });
            if(response.Status == ProtocolMethods.Error)
            {
                throw new Exception(response.Message);
            }
            string[] clients = response.Message.Split('\n');
            List<Client> clientsDataBase = new List<Client>();
            foreach(string client in clients)
            {
                if (!String.IsNullOrEmpty(client))
                {
                    Client newClient = new Client
                    {
                        Name = client.Split("Nombre:")[1].Trim()
                    };
                    clientsDataBase.Add(newClient);
                }
                
            }
            return clientsDataBase;
        }

        public async Task<Client> UpdateAsync(string name, Client newClient)
        {
            
            newClient.Validate();
            string request = name + "-"  + "-" + newClient.Name + "-" + newClient.Password;
            ResponseClient response = await user.UpdateUserAsync(new Request
            {
                Attributes = request,
                Name = "Admin"
            });
            if(response.Status == ProtocolMethods.Error)
            {
                throw new Exception(response.Message);
            }
            Client client = new Client
            {
                Name = response.Name,
                Password = response.Password
            };
            return client;

        }
    }
}
