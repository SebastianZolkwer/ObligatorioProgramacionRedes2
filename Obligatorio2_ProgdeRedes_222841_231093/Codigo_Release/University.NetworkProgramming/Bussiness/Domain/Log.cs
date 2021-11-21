using System;
using System.Collections.Generic;
using System.Text;

namespace Bussiness.Domain
{
    public class Log
    {
        public string ClientName { get; set; }
        public string GameTitle { get; set; }
        public string Date { get; set; }
        public string Message { get; set; }

        public Log(string log)
        {
            string[] data = log.Split("-");
            ClientName = data[0];
            GameTitle = data[1];
            Date = data[2];
            Message = data[3];
        }
    }
}
