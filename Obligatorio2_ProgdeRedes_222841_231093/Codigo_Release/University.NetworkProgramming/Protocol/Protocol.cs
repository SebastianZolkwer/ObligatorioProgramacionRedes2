using Files;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Protocol
{
    public class Protocol
    {
        public static int MethodLength = 2;
        public static int DataLength = 4;

        public const int FileNameSize = 4;
        public const int FileSize = 8;
        public const int MaxPacketSize = 8192;
        public async static Task SendAndCodeAsync(NetworkStream networkStream, int method, String message, string direction)
        {
            Header header = new Header(direction, method, message.Length);
            byte[] data = header.GetRequest();
            byte[] values = Encoding.UTF8.GetBytes(message);
            await SendSpecificMessageAsync(networkStream, data);
            await SendSpecificMessageAsync(networkStream, values);
        }

        public static long CalculateParts(long size)
        {
            long parts = size / MaxPacketSize;
            return parts * MaxPacketSize == size ? parts : parts + 1;
        }

        public async static Task SendSpecificMessageAsync(NetworkStream networkStream, byte[] dataLength)
        {
            await networkStream.WriteAsync(dataLength, 0, dataLength.Length);
        }

        public async static Task<Header> ReceiveAndDecodeFixDataAsync(NetworkStream networkStream, Header header)
        {
            int headerLength = ProtocolMethods.Request.Length + MethodLength + DataLength;
            byte[] data = await ReceiveDataAsync(networkStream, headerLength);
            header.DecodeData(data);
            return header;
        }

        public async static Task ReceiveFileAsync(NetworkStream networkStream, Header header, FileStreamHandler fileStreamHandler, bool servidor)
        {
            string data = await RecieveAndDecodeVariableDataAsync(networkStream, header.GetDataLength());
            string[] fileData = data.Split('!');
            string title = fileData[1];
            long fileSize = long.Parse(fileData[2]);
            long parts = CalculateParts(fileSize);
            long offset = 0;
            long currentPart = 1;
            var nameFile = fileData[0].Split('.');
            string type = nameFile[nameFile.Length - 1];
            while (fileSize > offset)
            {
                byte[] buffer;
                if (currentPart != parts)
                {
                    buffer = await ReceiveDataAsync(networkStream, MaxPacketSize);
                    offset += MaxPacketSize;
                }
                else
                {
                    int lastPartSize = (int)(fileSize - offset);
                    buffer = await ReceiveDataAsync(networkStream, lastPartSize);
                    offset += lastPartSize;
                }
                if (servidor)
                {
                    await fileStreamHandler.WriteDataAsync(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent + "/CaratulasServer/" + title + "." + type, buffer);
                }
                else
                {
                    await fileStreamHandler.WriteDataAsync("CaratulasClient/" + title + "." + type, buffer);
                }
                currentPart++;
            }
        }

        public async static Task<string> RecieveAndDecodeVariableDataAsync(NetworkStream networkStream, int length)
        {
            byte[] value = await ReceiveDataAsync(networkStream, length);
            string response = Encoding.UTF8.GetString(value);
            return response;
        }


        public async static Task<byte[]> ReceiveDataAsync(NetworkStream networkStream, int length)
        {
            int offset = 0;
            byte[] response = new byte[length];
            while (offset < length)
            {
                int received = await networkStream.ReadAsync(response, offset, length - offset);
                if (received == 0)
                {
                    throw new SocketException();
                }
                offset += received;
            }
            return response;
        }


        public async static Task SendFileAsync(NetworkStream networkStream, string path, FileHandler filehandler, int method, string title, FileStreamHandler fileStreamHandler)
        {
            long fileSize = filehandler.GetFileSize(path);
            string fileName = filehandler.GetFileName(path);
            var fileSizeData = BitConverter.GetBytes(fileSize).Length;
            var header = new Header(ProtocolMethods.Request, method, fileName.Length + fileSizeData + title.Length);
            var dataHeader = header.GetRequest();
            await SendSpecificMessageAsync(networkStream, dataHeader);
            string fileData = fileName + "!" + title + "!" + fileSize;
            var bytesFileData = Encoding.UTF8.GetBytes(fileData);
            await SendSpecificMessageAsync(networkStream, bytesFileData);
            long parts = CalculateParts(fileSize);
            long offset = 0;
            long currentPart = 1;

            while (fileSize > offset)
            {
                byte[] data;
                if (currentPart != parts)
                {
                    data = await fileStreamHandler.ReadDataAsync(@path, MaxPacketSize, offset);
                    offset += MaxPacketSize;
                }
                else
                {
                    int lastPartSize = (int)(fileSize - offset);
                    data = await fileStreamHandler.ReadDataAsync(@path, lastPartSize, offset);
                    offset += lastPartSize;
                }
                await SendSpecificMessageAsync(networkStream, data);
                currentPart++;
            }
        }
    }
}
