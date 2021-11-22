using Bussiness.Domain;
using System;

namespace Model
{
    public class ClientDto
    {
        public string Name { get; set; }

        public ClientDto(Client client)
        {
            Name = client.Name;
        }
    }
}
