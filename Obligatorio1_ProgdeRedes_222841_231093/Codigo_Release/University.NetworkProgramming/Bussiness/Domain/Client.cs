using System;
using System.Collections.Generic;
using System.Text;

namespace Bussiness.Domain
{
    public class Client
    {
        public List<Game> boughtGames;
        public string name;
        public string password;
        public bool active;

        public Client()
        {
            boughtGames = new List<Game>();
        }
        public Client(string name, string password)
        {
            this.name = name;
            this.password = password;
            boughtGames = new List<Game>();
            this.active = false;
        }
    }
    
}
