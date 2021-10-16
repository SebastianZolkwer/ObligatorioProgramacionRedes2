using Files;
using System;
using System.Net.Sockets;
using System.Text;

namespace Protocol
{
    public class Protocol
    {
        public static int MethodLength = 2;
        public static int DataLength = 4;

        public const int FileNameSize = 4;
        public const int FileSize = 8;
        public const int MaxPacketSize = 8192;
        public static void SendAndCode(Socket socket, int method, String message, string direction)
        {

            Header header = new Header(direction, method, message.Length);
            byte[] data = header.GetRequest();

            byte[] values = Encoding.UTF8.GetBytes(message);


            SendSpecificMessage(socket, data);
            SendSpecificMessage(socket, values);
        }



        public static long CalculateParts(long size)
        {
            long parts = size / MaxPacketSize;
            return parts * MaxPacketSize == size ? parts : parts + 1;
        }



        public static void SendSpecificMessage(Socket socket, byte[] dataLength)
        {
            int totalDataSent = 0;

            while (totalDataSent < dataLength.Length)
            {
                try
                {
                    int sent = socket.Send(dataLength, offset: totalDataSent, size: dataLength.Length - totalDataSent, SocketFlags.None);
                    if (sent == 0)
                    {
                        throw new SocketException();
                    }
                    totalDataSent += sent;
                } catch(Exception ex) { }
            }
        }



        public static Header ReceiveAndDecodeFixData(Socket handler, Header header)
        {
            int headerLength = ProtocolMethods.Request.Length + MethodLength + DataLength;
            byte[] data = ReceiveData(handler, headerLength);
            header.DecodeData(data);
            return header;
        }

        public static void ReceiveFile(Socket socket, Header header, FileStreamHandler fileStreamHandler, bool servidor)
        {
            string data = RecieveAndDecodeVariableData(socket, header.GetDataLength());



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
                    buffer = ReceiveData(socket, MaxPacketSize);
                    offset += MaxPacketSize;
                }
                else
                {
                    int lastPartSize = (int)(fileSize - offset);
                    buffer = ReceiveData(socket, lastPartSize);
                    offset += lastPartSize;
                }

                if (servidor)
                {
                    fileStreamHandler.WriteData("CaratulasServer/" + title + "." + type, buffer);
                }
                else
                {
                    fileStreamHandler.WriteData("CaratulasClient/" + title + "." + type, buffer);
                }

                currentPart++;
            }
        }

        public static string RecieveAndDecodeVariableData(Socket socket, int length)
        {
            byte[] value = ReceiveData(socket, length);
            string response = Encoding.UTF8.GetString(value);
            return response;
        }


        public static byte[] ReceiveData(Socket socket, int length)
        {
            int offset = 0;
            byte[] response = new byte[length];
            while (offset < length)
            {
                int received = socket.Receive(response, offset, length - offset, SocketFlags.None);
                if (received == 0)
                {
                    throw new SocketException();
                }
                offset += received;
            }
            return response;
        }


        public static void SendFile(Socket socket, string path, FileHandler filehandler, int method, string title, FileStreamHandler fileStreamHandler)
        {
            long fileSize = filehandler.GetFileSize(path);
            string fileName = filehandler.GetFileName(path);
            var fileSizeData = BitConverter.GetBytes(fileSize).Length;
            var header = new Header(ProtocolMethods.Request, method, fileName.Length + fileSizeData + title.Length);
            var dataHeader = header.GetRequest();
            SendSpecificMessage(socket, dataHeader);
            string fileData = fileName + "!" + title + "!" + fileSize;
            var bytesFileData = Encoding.UTF8.GetBytes(fileData);
            SendSpecificMessage(socket, bytesFileData);

            long parts = CalculateParts(fileSize);
            long offset = 0;
            long currentPart = 1;

            while (fileSize > offset)
            {
                byte[] data;
                if (currentPart != parts)
                {
                    data = fileStreamHandler.ReadData(@path, MaxPacketSize, offset);
                    offset += MaxPacketSize;
                }
                else
                {
                    int lastPartSize = (int)(fileSize - offset);
                    data = fileStreamHandler.ReadData(@path, lastPartSize, offset);
                    offset += lastPartSize;
                }
                try
                {
                    SendSpecificMessage(socket, data);
                }
                catch (Exception ex)
                {

                } 
               
                currentPart++;
            }
        }

    }
}
