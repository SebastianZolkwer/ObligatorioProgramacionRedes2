using Bussiness.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicInterface.Interfaces
{
    public interface IClientLogic
    {
        Client Create(Client client);
        Client Get(string name);
        Client Update(Client client);
        Client Delete(string name);
        string Buy(string gameTitle);
    }
}
