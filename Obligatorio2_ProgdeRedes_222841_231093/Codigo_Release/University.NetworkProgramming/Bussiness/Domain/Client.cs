using System;
using System.Collections.Generic;
using System.Text;

namespace Bussiness.Domain
{
    public class Client
    {
        public List<Game> BoughtGames;
        public string Name;
        public string Password;
        public bool Active;

        public Client()
        {
            BoughtGames = new List<Game>();
        }
        public Client(string name, string password)
        {
            Name = name;
            Password = password;
            BoughtGames = new List<Game>();
            Active = false;
        }
        public void Validate()
        {
            if (String.IsNullOrEmpty(this.Name))
            {
                throw new InvalidOperationException("El nombre no puede ser nulo or vacio");
            }
            if (String.IsNullOrEmpty(this.Password))
            {
                throw new InvalidOperationException("La contraseña no puede ser nula or vacia");
            }
        }
    }
    
}
