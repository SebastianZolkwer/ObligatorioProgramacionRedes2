using Bussiness.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicInterface.Interfaces
{
    public interface IGameLogic
    {
        Task<Game> CreateAsync(Game game);
        Task<Game> GetAsync(string title);
        Task<string> UpdateAsync(string title, Game game);
        Task<string> DeleteAsync(string title);
        Task<List<Game>> GetAllAsync();
    }
}
