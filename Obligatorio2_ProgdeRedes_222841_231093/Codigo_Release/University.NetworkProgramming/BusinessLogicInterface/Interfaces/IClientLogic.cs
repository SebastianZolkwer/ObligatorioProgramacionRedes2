using Bussiness.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicInterface.Interfaces
{
    public interface IClientLogic
    {
        Task<Client> CreateAsync(Client client);
        Task<String> UpdateAsync(string name, Client newClient);
        Task<string> DeleteAsync(string name);
        Task<string> BuyGameAsync(string name, string title);
        Task<List<Client>> GetAllAsync();
        Task<string> ReturnGameAsync(string name, string title);
    }
}
