using Bussiness.Domain;
using DataAccessInterface;
using System;
using System.Collections.Generic;

namespace DataAccess
{
    public class LogRepository : ILogRepository
    {
        private List<Log> logs;
    }
}
