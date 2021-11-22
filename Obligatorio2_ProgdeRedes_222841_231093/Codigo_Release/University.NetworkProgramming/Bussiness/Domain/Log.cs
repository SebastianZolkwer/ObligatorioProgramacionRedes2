using System;
using System.Collections.Generic;
using System.Text;

namespace Bussiness.Domain
{
    public class Log
    {
        public string clientName { get; set; }
        public string gameTitle { get; set; }
        public string date { get; set; }
        public string message { get; set; }

        public Log(string log)
        {
            string[] data = log.Split("-");
            clientName = data[0];
            gameTitle = data[1];
            date = data[2];
            message = data[3];
        }
    }
}
