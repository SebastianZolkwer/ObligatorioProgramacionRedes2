using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol
{
    public static class ProtocolMethods
    {
        public const int Error = 0;
        public const int Success = 1;
        public const int Create = 1;
        public const int Update = 2;
        public const int Buy = 3;
        public const int Evaluate = 4;
        public const int Search = 5;
        public const int Show = 6;
        public const int ShowAll = 7;
        public const int Reviews = 8;
        public const int Delete = 9;
        public const int Exit = 10;
        public const int SendImage = 11;
        public const int UpdateRoute = 12;
        public const int ReceiveImage = 13;
        public const string Request = "REQ";
        public const string Response = "RES";
    }
}


