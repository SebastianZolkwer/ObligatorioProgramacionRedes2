using BusinessLogicInterface.Interfaces;
using Bussiness.Domain;
using Grpc.Net.Client;
using ServerAdmin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Logic
{
    public class GameLogic : IGameLogic
    {
        private static GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:5001");
        private static Greeter.GreeterClient user = new Greeter.GreeterClient(channel);
        public async Task<Game> CreateAsync(Game game)
        {
            try
            {
                game.Validate();
            }catch(InvalidOperationException e)
            {
                throw new Exception(e.Message);
            }
            string request =  game.Title + "-" + game.Gender + "-" + game.Sinopsis;
            Response response = await user.CreateGameAsync(new Request
            {
                Attributes = request
            });

        }

        public async Task<string> DeleteAsync(string title)
        {
            Response response = await user.DeleteGameAsync(new Request
            {
                Attributes = title
            });
        }

        public async Task<Game> GetAsync(string title)
        {
            Response response = await user.ShowAsync(new Request
            {
                Attributes = title
            });
        }

        public async Task<Game> UpdateAsync(string title,Game game)
        {
            string request = title + "-" + game.Title + "-" + game.Gender + "-" + game.Sinopsis;
            Response response = await user.UpdateGameAsync(new Request
            {
                Attributes = request
            });
        }

        public async Task<List<Game>> GetAllAsync()
        {
            Response response = await user.ShowAllGamesAsync(new Request
            {
                Attributes = ""
            });
        }
    }
}
