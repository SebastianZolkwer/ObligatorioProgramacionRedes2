using Server;
using System;
using System.Threading.Tasks;

namespace SocketsSimpleServer
{
    class ServerProgram
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting server");
            var serverHandler = new ServerHandler();
            await serverHandler.StartServer();
        }
    }
}
