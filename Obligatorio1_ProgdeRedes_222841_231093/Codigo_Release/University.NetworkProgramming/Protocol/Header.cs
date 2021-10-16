using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol
{
    public class Header
    {
        private byte[] direction;
        private byte[] method;
        private byte[] dataLength;

        private string _direction;
        private int _method;
        private int _dataLength;

        public Header(string status, int _method, int datalength)
        {
            direction = Encoding.UTF8.GetBytes(status);
            string stringMethod = _method.ToString("D2");
            method = Encoding.UTF8.GetBytes(stringMethod);
            string stringData = datalength.ToString("D4");
            dataLength = Encoding.UTF8.GetBytes(stringData);
        }

        public Header()
        {
        }

        public byte[] GetRequest()
        {
            byte[] header = new byte[ProtocolMethods.Request.Length + Protocol.MethodLength + Protocol.DataLength];
            direction.CopyTo(header, 0);
            method.CopyTo(header,direction.Length);
            dataLength.CopyTo(header, direction.Length + method.Length);
            return header;
        }

        public bool DecodeData(byte[] data)
        {
            _direction = Encoding.UTF8.GetString(data, 0, ProtocolMethods.Request.Length);
            var command = Encoding.UTF8.GetString(data, ProtocolMethods.Request.Length, Protocol.MethodLength);
            _method = int.Parse(command);
            var dataLength = Encoding.UTF8.GetString(data, ProtocolMethods.Request.Length + Protocol.MethodLength, Protocol.DataLength);
            _dataLength = int.Parse(dataLength);
            return true;
        }

        public int GetDataLength()
        {
            return _dataLength;
        }

        public int GetMethod()
        {
            return _method;
        }
    }
}
