using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Squaddie.Serialization
{
    internal static class ByteConversionUtility
    {
        public static int ReadInt(MemoryStream data)
        {
            byte[] buffer = new byte[4];
            data.Read(buffer, 0, 4);

            return BitConverter.ToInt32(buffer, 0);
        }

        public static string ReadString(MemoryStream data)
        {
            int length = ReadInt(data);

            if (length > 0)
            {
                byte[] buffer = new byte[length];
                data.Read(buffer, 0, length);

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

        public static void ReadPadding(MemoryStream data)
        {
            if (ReadInt(data) != 0)
            {
                throw new Exception("Padding Error: Did not Read Expected Integer 0");
            }
        }

        public static byte[] BytePadding()
        {
            return ByteInt(0);
        }

        public static byte[] ByteInt(int number)
        {
            //Possibly need to ensure correct endian layout
            byte[] numberInBytes = BitConverter.GetBytes(number);
            Array.Resize(ref numberInBytes, 4);
            return numberInBytes;
        }

        public static byte[] ByteString(string words)
        {
            byte[] stringInBytes = Encoding.GetEncoding("ISO-8859-1").GetBytes((words + "\0"));

            List<byte> lengthThenString = ByteInt(stringInBytes.Length).ToList();
            lengthThenString.AddRange(stringInBytes);

            return lengthThenString.ToArray();
        }
    }
}
