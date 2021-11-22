using Bussiness.Domain;
using DataAccessInterface;
using System;
using System.Collections.Generic;

namespace DataAccess
{
    public class LogRepository
    {
        private static List<Log> logs = new List<Log>();

        public static void Add(Log log)
        {
            logs.Add(log);
        }

        public static  List<Log> GetAll()
        {
            return logs;
        }
    }
}
