using BusinessLogicInterface.Interfaces;
using Bussiness.Domain;
using Grpc.Net.Client;
using Model;
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
        public string Buy(string gameTitle)
        {
            throw new NotImplementedException();
        }

        public Task<string> BuyAsync(UserBuyGame gameBuy)
        {
            
        }

        public async Task<Client> CreateAsync(Client client)
        {
            try
            {
                client.Validate();
            }
            catch (InvalidOperationException e)
            {
                throw new Exception(e.Message);
            }
            string request =  client.name + "-" + client.password;
            ResponseClient responseClient = await user.RegisterAsync(new Request
            {
                Attributes = request
            });
        }

        public async Task<string> DeleteAsync(Client client)
        {
            try
            {
                client.Validate();
            }
            catch (InvalidOperationException e)
            {
                throw new Exception(e.Message);
            }

            string request = client.name + "-" + client.password;
            Response response = await user.DeleteUserAsync(new Request
            {
                Attributes = request
            });
        }


        public async Task<List<Client>> GetAllAsync()
        {
            Response response = await user.ShowAllUsersAsync(new Request
            {
                Attributes = ""
            });
        }

        public async Task<Client> UpdateAsync(Client oldClient, Client client)
        {
            try
            {
                client.Validate();
                oldClient.Validate();
            }
            catch (InvalidOperationException e)
            {
                throw new Exception(e.Message);
            }
            string request = oldClient.name + "-" + oldClient.password + "-" + client.name + "-" + client.password;
            Response response = await user.UpdateUserAsync(new Request
            {
                Attributes = request
            });
        }
    }
}
