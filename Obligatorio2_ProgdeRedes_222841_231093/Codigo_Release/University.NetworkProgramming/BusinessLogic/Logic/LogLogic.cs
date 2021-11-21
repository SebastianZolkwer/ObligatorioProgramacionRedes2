using BusinessLogicInterface.Interfaces;
using Bussiness.Domain;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Logic
{
    public class LogLogic : ILogLogic
    {

        public LogLogic(){
        }

        public List<Log> GetAll(string userName, string gameTitle, string date)
        {
            List<Log> _logs = LogRepository.GetAll();
            if (!String.IsNullOrEmpty(userName))
            {
                _logs = _logs.Where(log => log.ClientName.Contains(userName)).ToList();
            }
            if (!String.IsNullOrEmpty(gameTitle))
            {
                _logs = _logs.Where(log => log.GameTitle.Contains(gameTitle)).ToList();
            }
            if (!String.IsNullOrEmpty(date))
            {
                _logs = _logs.Where(log => log.Date.Contains(date)).ToList();
            }
            return _logs;
        }
    }
}
