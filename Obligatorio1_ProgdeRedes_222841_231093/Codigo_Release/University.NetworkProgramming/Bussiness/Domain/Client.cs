using System;
using System.Collections.Generic;
using System.Text;

namespace Bussiness.Domain
{
    public class Client
    {
        public List<Game> boughtGames;

        public Client()
        {
            boughtGames = new List<Game>();
        }
    }
    
}
