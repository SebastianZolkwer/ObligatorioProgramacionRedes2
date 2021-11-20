using Bussiness.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicInterface.Interfaces
{
    public interface ILogLogic
    {
        List<Log> GetAll(string userName, string gameTitle, string date);
    }
}
