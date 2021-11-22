using Bussiness.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Bussiness
{
    public static class Logic
    {
        public static List<Game> games = new List<Game>();
        private static List<Client> clients = new List<Client>();
        private static readonly object gamesLock = new Object();
        private static readonly object clientsLock = new Object();
        public static string Add(string information)
        {

            string[] data = information.Split("-");

            Game newGame = new Game
            {
                Qualifications = new List<Review>(),
                Title = data[0].Trim(),
                Gender = data[1].Trim(),
                Sinopsis = data[2].Trim(),
                AverageQualification = 0,
                SumOfQualification = 0
            };
            lock (gamesLock)
            {
                if (GetGame(data[0]) != null) throw new Exception("Ya existe un juego con ese nombre\n");
                games.Add(newGame);
            }
            return "Fue agregado el juego\n";
        }

        public static Client Register(string request)
        {
            string[] data = request.Split("-");
            string name = data[0];
            string password = data[1];
            lock (clientsLock)
            {
                if (clients.Any(c => c.Name == name))
                {
                    throw new Exception("Ya existe un usuario con ese nombre.\n");
                }
                Client client = new Client(name, password);
                clients.Add(client);
                if (client != null)
                {
                    ActiveUser(client);
                }
                return client;
            }
        }

        private static Game GetGame(string title)
        {
            Game game = games.FirstOrDefault(game => game.Title == title.Trim());
            return game;
        }

        private static Client GetClient(string name)
        {
            Client user = clients.FirstOrDefault(client => client.Name == name);
            return user;
        }

        public static string Update(string information)
        {
            string[] data = information.Split("-");
            lock (gamesLock)
            {
                Game game = GetGame(data[0]);
                if (game == null) throw new Exception("No existe el juego seleccionado para modificar\n");
                if (String.IsNullOrEmpty(data[1]) && String.IsNullOrEmpty(data[2]) && String.IsNullOrEmpty(data[3])) throw new Exception("No paso ningun parametro para modificar el juego\n");
                if (!String.IsNullOrEmpty(data[1]))
                {
                    if (GetGame(data[1]) != null)
                    {
                        throw new Exception("Ya existe un juego con ese nombre\n");
                    }
                    game.Title = data[1];
                }
                if (!String.IsNullOrEmpty(data[2]))
                {
                    game.Gender = data[2];
                }
                if (!String.IsNullOrEmpty(data[3]))
                {
                    game.Sinopsis = data[3];
                }
                return "Fue modificado el juego\n";
            }
        }

        public static Client Login(string request)
        {
            string[] data = request.Split("-");
            string name = data[0];
            string password = data[1];
            lock (clientsLock)
            {
                Client client = GetClient(name);
                if (client is null)
                {
                    throw new Exception("No existe usuario con ese nombre, reescriba o registrese.\n");
                }
                if (client.Password != password)
                {
                    throw new Exception("Password incorrecta");
                }
                if (client != null)
                {
                    ActiveUser(client);
                }
                return client;
            }
        }

        public static string GetAll()
        {
            string response = "";
            lock (gamesLock)
            {
                if (games.Count == 0)
                {
                    throw new Exception("No hay juegos en el sistema\n");
                }
                else
                {
                    response += "Lista de juegos:\n";
                    {
                        foreach (Game game in games)
                        {
                            response += " -Titulo:" + game.Title + "-Genero:" + game.Gender + "\n";
                        }
                    }
                }
            }
            return response;
        }

        public static string LogOut(string name)
        {
            lock (clientsLock)
            {
                Client client = GetClient(name);
                DisactivateUser(client);
                return "Hasta pronto!!\n";
            }
        }

        public static string ShowAllUsers()
        {
            string response = "";
            int clientsNumber = 1;
            lock (clientsLock)
            {
                if (clients.Count == 0)
                {
                    throw new Exception("No existen usuarios registrados en el sistema");
                }
                foreach (Client client in clients)
                {
                    response = response + clientsNumber + ". Nombre: " + client.Name + "\n";
                    clientsNumber++;
                }
            }
            return response;

        }

        public static string Delete(string data)
        {
            lock (gamesLock)
            {
                Game game = GetGame(data);
                if (game == null)
                {
                    throw new Exception("No fue encontrado el juego \n");
                }
                games.Remove(game);
                foreach (Client client in clients)
                {
                    Game deleteGame = client.BoughtGames.FirstOrDefault(game => game.Title == data.Trim());
                    if (deleteGame != null)
                    {
                        client.BoughtGames.Remove(deleteGame);
                    }
                }
            }
            return "Fue borrado el juego\n";
        }

        public static string Evaluate(string information)
        {
            string[] data = information.Split("-");
            lock (gamesLock)
            {
                Game game = GetGame(data[0]);
                if (game == null)
                {
                    throw new Exception("No fue encontrado el juego\n");
                }
                int rate = GetNumber(data[1]);
                if (rate < 0 || rate > 10)
                {
                    throw new Exception("La calificacion debe ser un numero entre 1 y 10\n");
                }
                Review review = new Review
                {
                    Rating = rate,
                    Description = data[2].Trim()
                };
                game.Qualifications.Add(review);
                game.SumOfQualification += rate;
                game.AverageQualification = game.SumOfQualification / game.Qualifications.Count;
            }
            return "Se evaluo el juego\n";
        }

        public static string Search(string filters)
        {
            string[] data = filters.Split("-");
            lock (gamesLock)
            {
                List<Game> gamesToShow = games;
                if (!String.IsNullOrEmpty(data[0]))
                {
                    gamesToShow = gamesToShow.FindAll(game => game.Title == data[0]);
                }
                if (!String.IsNullOrEmpty(data[1]))
                {
                    int qualification = GetNumber(data[1]);
                    gamesToShow = gamesToShow.FindAll(game => game.AverageQualification == qualification);
                }
                if (!String.IsNullOrEmpty(data[2]))
                {
                    gamesToShow = gamesToShow.FindAll(game => game.Gender == data[2]);
                }
                if (gamesToShow.Count == 0)
                {
                    throw new Exception("No se encontraron juegos con esos filtros\n");
                }
                string response = "";
                foreach (Game game in gamesToShow)
                {
                    response += "-Titulo:" + game.Title + " Genero: " + game.Gender + "\n";
                }
                return response;
            }
        }

        public static string GetRouteImage(string request)
        {
            lock (gamesLock)
            {
                Game game = GetGame(request);
                if (game == null)
                {
                    throw new Exception("No fue encontrado el juego\n");
                }
                string imgRoute = game.ImageRoute;
                if (imgRoute is null)
                {
                    throw new Exception("El juego seleccionado aun no tiene caratula");
                }
                return imgRoute;
            }
        }

        public static void ActiveUser(Client client)
        {
            lock (clientsLock)
            {
                client.Active = true;
            }
        }

        public static void DisactivateUser(Client client)
        {
            lock (clientsLock)
            {
                client.Active = false;
            }
        }

        public static string GetListBought(string name)
        {
            Client client = GetClient(name);
            if (client.BoughtGames.Count == 0)
            {
                throw new Exception("No hay juegos comprados \n");
            }
            else
            {
                string response = "Lista de juegos:\n";
                foreach (Game game in client.BoughtGames)
                {
                    response += " -Titulo:" + game.Title + " Genero:" + game.Gender + "\n";
                }
                return response;
            }
        }

        public static string DeleteUser(string name)
        {
            lock (clientsLock)
            {
                Client client = clients.FirstOrDefault(c => c.Name == name);
                if (client is null)
                {
                    throw new Exception("No fue encontrado el usuario con ese nombre y contrasena.\n");
                }
                if (client.Active)
                {
                    throw new Exception("No es posible eliminar usuarios activos.\n");
                }
                clients.Remove(client);
            }
            return "Se ha eliminado el usuario " + name;
        }

        public static void UpdateRoute(string request)
        {
            string[] data = request.Split("-");
            lock (gamesLock)
            {
                Game game = GetGame(data[0]);
                if (game == null)
                {
                    throw new Exception("No fue encontrado el juego\n");
                }
                game.ImageRoute = Directory.GetCurrentDirectory() + @"\CaratulasServer\" + data[0] + data[1];
            }
        }

        public static string UpdateUser(string request)
        {
            string[] data = request.Split("-");
            string oldName = data[0];
            string newName = data[1];
            string newPassword = data[2];
            lock (clientsLock)
            {
                Client client = clients.FirstOrDefault(c => c.Name == oldName);
                if (client is null)
                {
                    throw new Exception("No fue encontrado el usuario con ese nombre y contrasena.\n");
                }
                client.Name = newName;
                client.Password = newPassword;
            }
            return "Se ha modificado el usuario.";
        }

        public static string Show(string data)
        {
            Game game;
            lock (gamesLock)
            {
                game = GetGame(data);
            }
            if (game == null)
            {
                throw new InvalidOperationException("No fue encontrado el juego\n");
            }
            string response = "";
            response += "Titulo:" + game.Title + "-" + "Genero:" + game.Gender + "-" + "Sinopsis:" + game.Sinopsis + "-Promedio de calificacion:" + game.AverageQualification;
            string reviews = GetReviews(game.Title);
            if (reviews == "")
            {
                response += "-No hay reviews para este juego";
            }
            else
            {
                response += " Reviews:" + reviews;
            }
            if (game.ImageRoute is null)
            {
                throw new InvalidDataException(response);
            }
            return response + "\n";
        }

        public static string GetReviews(string data)
        {
            Game game;
            lock (gamesLock)
            {
                game = GetGame(data);
            }
            if (game == null)
            {
                throw new Exception("No fue encontrado el juego\n");
            }
            string response = "";
            foreach (Review review in game.Qualifications)
            {
                response += "-" + review.Rating.ToString() + " " + review.Description;
            }
            return response;
        }

        public static string Buy(string data, string name)
        {
            Game game;
            lock (clientsLock)
            {
                Client client = GetClient(name);
                lock (gamesLock)
                {
                    game = GetGame(data);
                }
                if (game == null)
                {
                    throw new Exception("No fue encontrado el juego\n");
                }
                else if (client.BoughtGames.FirstOrDefault(game => game.Title == data.Trim()) != null)
                {
                    throw new Exception("Ya se compro este juego\n");
                }
                client.BoughtGames.Add(game);
                return "Se compro el juego\n";
            }

        }

        public static string DeleteFromBought(string data, string name)
        {
            Game game;
            lock (clientsLock)
            {
                Client client = GetClient(name);
                lock (gamesLock)
                {
                    game = client.BoughtGames.FirstOrDefault(game => game.Title == data.Trim());
                }
                if (game == null)
                {
                    throw new Exception("No fue encontrado el juego\n");
                }
                client.BoughtGames.Remove(game);
                return "Se elimino el juego\n";
            }
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
                throw new Exception("La calificacion debe ser un numero\n");
            }
        }

        public static Client RegisterWithoutActivate(string request)
        {
            string[] data = request.Split("-");
            string name = data[0];
            string password = data[1];
            lock (clientsLock)
            {
                if (clients.Any(c => c.Name == name))
                {
                    throw new Exception("Ya existe un usuario con ese nombre.\n");
                }
                Client client = new Client(name, password);
                clients.Add(client);
                return client;
            }
        }
    }
}
