using Bussiness.Domain;
using System;
using System.Collections.Generic;

namespace DataAccessInterface
{
    public interface ILogRepository
    {
        void Add(Log log);
        List<Log> GetAll();
    }
}
