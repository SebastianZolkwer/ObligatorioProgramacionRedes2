using Bussiness.Domain;
using DataAccessInterface;
using System;
using System.Collections.Generic;

namespace DataAccess
{
    public class LogRepository : ILogRepository
    {
        private List<Log> logs;

        public void Add(Log log)
        {
            logs.Add(log);
        }

        public List<Log> GetAll()
        {
            return logs;
        }
    }
}
