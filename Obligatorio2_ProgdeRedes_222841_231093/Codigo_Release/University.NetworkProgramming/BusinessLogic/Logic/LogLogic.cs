using BusinessLogicInterface.Interfaces;
using Bussiness.Domain;
using DataAccessInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Logic
{
    public class LogLogic : ILogLogic
    {
        public ILogRepository _logRepository;
        public LogLogic(ILogRepository logRepository){
            this._logRepository = logRepository;
        }

        public List<Log> GetAll(string userName, string gameTitle, string date)
        {
           
        }
    }
}
