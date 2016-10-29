using System;
using System.IO;
using System.Linq;
using System.Numerics;

namespace CipherManager.Core
{
    /// <summary>
    /// 用于存储最基础密钥的类
    /// </summary>
    public class CipherObject : IConvertibleWithByte
    {
        private byte[] content;

        public byte[] Content
        {
            get
            {
                return content ?? (content = new byte[0]);
            }
        }

        public byte[] Bytes
        {
            get
            {
                return this.Content;
            }
        }

        public string Base64String
        {
            get
            {
                return Convert.ToBase64String(this.Content);
            }
        }

        public string HexString
        {
            get
            {
                return BitConverter.ToString(this.Content);
            }
        }

        public BigInteger Integer
        {
            get
            {
                return this.GetInteger(true);
            }
        }

        public CipherObject()
        {
        }

        public CipherObject(byte[] content)
        {
            if (content == null)
            {
                throw new ArgumentNullException();
            }
            this.content = content;
        }

        public BigInteger GetInteger(bool ignoreSign = true)
        {
            byte[] array;
            if (ignoreSign)
            {
                array = new byte[this.Content.Length + 1];
                Buffer.BlockCopy(this.Content, 0, array, 1, this.Content.Length);
                Array.Reverse(array);
                array[array.Length - 1] = 0;
            }
            else
            {
                array = new byte[this.Content.Length];
                Buffer.BlockCopy(this.Content, 0, array, 0, this.Content.Length);
                Array.Reverse(array);
            }
            return new BigInteger(array);
        }

        public string GetHexString(string split = "")
        {
            return this.HexString.Replace("-", "");
        }

        public override string ToString()
        {
            return this.HexString;
        }

        public bool Equals(CipherObject other)
        {
            return this.Content.SequenceEqual(other.Content);
        }

        public override bool Equals(object obj)
        {
            return obj is CipherObject && this.Equals((CipherObject)obj);
        }

        public static bool operator ==(CipherObject first, CipherObject second)
        {
            return object.Equals(first, second);
        }

        public static bool operator !=(CipherObject first, CipherObject second)
        {
            return !object.Equals(first, second);
        }

        public override int GetHashCode()
        {
            return this.Content.GetHashCode();
        }

        public byte[] ToByteArray()
        {
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.WriteDataAndLength(this.content);
                    result = memoryStream.ToArray();
                }
            }
            return result;
        }

        public void FromByteArray(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                {
                    this.content = binaryReader.ReadBytesWithLength();
                }
            }
        }
    }
}
