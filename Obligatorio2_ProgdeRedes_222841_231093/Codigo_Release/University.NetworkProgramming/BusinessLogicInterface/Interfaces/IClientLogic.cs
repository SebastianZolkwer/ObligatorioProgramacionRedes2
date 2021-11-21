using Bussiness.Domain;
using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicInterface.Interfaces
{
    public interface IClientLogic
    {
        Task<Client> CreateAsync(Client client);
        Task<Client> UpdateAsync(Client oldClient,Client newClient);
        Task<string> DeleteAsync(Client client);
        Task<string>  BuyAsync(UserBuyGame gamebuy);
        Task<List<Client>> GetAllAsync();
    }
}
