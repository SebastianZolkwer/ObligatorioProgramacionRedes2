
using System.Threading.Tasks;

namespace SocketSimpleClient
{
    class ClientProgram
    {

        static async Task Main(string[] args)
        {
            ClientHandler clientHandler = new ClientHandler();
            await clientHandler.StartClientAsync();
        }
         
    }
}
