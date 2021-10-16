using Server;
using System;

namespace SocketsSimpleServer
{
    class ServerProgram
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Starting server");
            var serverHandler = new ServerHandler();
        }

    }
}
