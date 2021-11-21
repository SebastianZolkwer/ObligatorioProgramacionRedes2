using BusinessLogicInterface.Interfaces;
using Bussiness.Domain;
using Grpc.Net.Client;
using Protocol;
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
            GameResponse response = await user.CreateGameAsync(new Request
            {
                Attributes = request,
                Name = "Admin"
            });
            if (response.Status == ProtocolMethods.Error)
            {
                throw new Exception(response.Message);
            }
            Game newGame = new Game
            {
                Title = response.Title,
                Gender = response.Gender,
                Sinopsis = response.Sinopsis
            };
            return newGame;
        }

    public async Task<string> DeleteAsync(string title)
        {
            Response response = await user.DeleteGameAsync(new Request
            {
                Attributes = title,
                Name = "Admin"
            });
            if(response.Status == ProtocolMethods.Error)
            {
                throw new Exception(response.Message);
            }
            return response.Message;
        }

        public async Task<Game> GetAsync(string title)
        {
            Response response = await user.ShowAsync(new Request
            {
                Attributes = title,
                Name = "Admin"
            });
            if(response.Status == ProtocolMethods.Error)
            {
                throw new Exception(response.Message);
            }
            string[] gameAttributes = response.Message.Split(":");
            Game game = new Game
            {
                Title = gameAttributes[1].Split(" ")[0],
                Gender = gameAttributes[2].Split(" ")[0],
                Sinopsis = gameAttributes[3].Split(" ")[0],
                AverageQualification = Int32.Parse(gameAttributes[4]),
            };
            return game;
        }

        public async Task<Game> UpdateAsync(string title,Game game)
        {
            string request = title + "-" + game.Title + "-" + game.Gender + "-" + game.Sinopsis;
            GameResponse response = await user.UpdateGameAsync(new Request
            {
                Attributes = request,
                Name = "Admin"
            });
            if (response.Status == ProtocolMethods.Error)
            {
                throw new Exception(response.Message);
            }
            Game newGame = new Game
            {
                Title = response.Title,
                Gender = response.Gender,
                Sinopsis = response.Sinopsis
            };
            return newGame;
        }

        public async Task<List<Game>> GetAllAsync()
        {
            Response response = await user.ShowAllGamesAsync(new Request
            {
                Attributes = "",
                 Name = "Admin"
            });
            if (response.Status == ProtocolMethods.Error)
            {
                throw new Exception(response.Message);
            }
            string[] onlyGames = response.Message.Split("Lista de juegos:\n");
            List<Game> gamesDataBase = new List<Game>();
            string[] games = onlyGames[1].Split('\n');

            foreach (string gameToShow in games)
            {
                if (!String.IsNullOrEmpty(gameToShow))
                {
                    string[] game = gameToShow.Split(':');
                    Game newGame = new Game
                    {
                        Title = game[1].Split('-')[0],
                        Gender = game[2]
                    };
                    gamesDataBase.Add(newGame);
                }
            }
            return gamesDataBase;
        }
    }
}
