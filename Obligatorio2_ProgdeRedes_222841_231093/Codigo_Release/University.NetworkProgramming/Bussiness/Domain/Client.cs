using System;
using System.Collections.Generic;
using System.Text;

namespace Bussiness.Domain
{
    public class Client
    {
        public List<Game> BoughtGames { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }

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
                throw new InvalidOperationException("El nombre no puede ser nulo o vacio");
            }
            if (String.IsNullOrEmpty(this.Password))
            {
                throw new InvalidOperationException("La contraseña no puede ser nula o vacia");
            }
        }
    }
}
