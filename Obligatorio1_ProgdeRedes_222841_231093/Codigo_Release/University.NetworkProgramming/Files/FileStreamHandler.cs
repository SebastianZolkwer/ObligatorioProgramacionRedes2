﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Files
{
    public class FileStreamHandler
    {
        public byte[] ReadData(string path, int length, long position)
        {
            byte[] response = new byte[length];
            using FileStream fileStream = new FileStream(path, FileMode.Open) { Position = position };
            int offset = 0;
            while (offset < length)
            {
                int read = fileStream.Read(response, offset, length - offset);
                if (read == 0)
                {
                    throw new Exception("Cannot read file");
                }

                offset += read;
            }

            return response;
        }

        public void WriteData(string path, byte[] data)
        {
            if (File.Exists(path))
            {
                using FileStream fileStream = new FileStream(path, FileMode.Append);
                fileStream.Write(data, 0, data.Length);
            }
            else
            {
                using FileStream fileStream = new FileStream(path, FileMode.Create);
                fileStream.Write(data, 0, data.Length);
            }
        }
    }
}
