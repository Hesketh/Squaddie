using System;
using System.IO;
using System.Text;

namespace Squaddie.Serialization
{
    internal class BinaryPoolReader
    {
        private MemoryStream dataStream;

        public BinaryPoolReader(byte[] data)
        {
            LoadData(data);
        }

        public void LoadData(byte[] data)
        {
            dataStream = new MemoryStream(data);
            dataStream.Seek(0, SeekOrigin.Begin);
        }

        public void ClearData()
        {
            dataStream = null;
        }

        public bool EndOfFile()
        {
            return dataStream.Position == dataStream.Length;
        }

        public byte[] ReadBytes(int amountOfBytes)
        {
            byte[] data = new byte[amountOfBytes];
            dataStream.Read(data, 0, amountOfBytes);

            return data;
        }

        public int ReadInt()
        {
            byte[] buffer = new byte[4];
            dataStream.Read(buffer, 0, 4);

            return BitConverter.ToInt32(buffer, 0);
        }

        public bool ReadBool()
        {
            byte[] buffer = new byte[1];
            dataStream.Read(buffer, 0, 1);

            return BitConverter.ToBoolean(buffer, 0);
        }

        public string ReadString()
        {
            int length = ReadInt();

            if (length > 0)
            {
                byte[] buffer = new byte[length];
                dataStream.Read(buffer, 0, length);

                return Encoding.GetEncoding("ISO-8859-1").GetString(buffer, 0, length - 1);
            }
            else if (length == 0)
            {
                return "";
            }
            else
            {
                throw new Exception("String Error: Specified Length was Less than 0");
            }
        }

        public void ReadPadding()
        {
            if (ReadInt() != 0)
            {
                throw new Exception("Padding Error: Did not Read Expected Integer 0");
            }
        }
    }
}