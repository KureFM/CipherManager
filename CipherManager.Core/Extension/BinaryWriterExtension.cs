using System;
using System.IO;

namespace CipherManager.Core
{
    public static class BinaryWriterExtension
    {
        public static void WriteDataAndLength(this BinaryWriter bw, byte[] data)
        {
            if (!bw.BaseStream.CanWrite)
            {
                throw new ArgumentException("bw不可写");
            }
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            bw.Write(data.Length);
            bw.Write(data);
        }

        public static void WriteDataAndLength(this BinaryWriter bw, string s, string encodingName = "Unicode")
        {
            byte[] data = s.Decode(encodingName);
            bw.WriteDataAndLength(data);
        }

        public static void WriteDataAndLength(this BinaryWriter bw, IConvertibleWithByte icwb)
        {
            if (icwb == null)
            {
                throw new ArgumentNullException("icwb");
            }
            byte[] data = icwb.ToByteArray();
            bw.WriteDataAndLength(data);
        }

        public static void Write(this BinaryWriter bw, DateTime dt)
        {
            byte[] bytes = BitConverter.GetBytes(dt.Ticks);
            bw.Write(bytes);
        }

        public static void Write(this BinaryWriter bw, Guid guid)
        {
            byte[] buffer = guid.ToByteArray();
            bw.Write(buffer);
        }
    }
}
