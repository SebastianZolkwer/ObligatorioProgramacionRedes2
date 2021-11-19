using Bussiness.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicInterface.Interfaces
{
    public interface IGameLogic
    {
        Game Create(Game game);
        Game Get(string title);
        Game Update(Game game);
        Game Delete(string title);
    }
}
