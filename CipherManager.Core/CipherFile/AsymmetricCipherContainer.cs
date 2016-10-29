using System;
using System.IO;

namespace CipherManager.Core
{
    /// <summary>
    /// 非对称密钥密钥容器
    /// </summary>
	public class AsymmetricCipherContainer : IConvertibleWithByte
	{
        /// <summary>
        /// 密钥信息
        /// </summary>
		public CipherInfo Info
		{
			get;
			private set;
		}

        /// <summary>
        /// 密钥内容
        /// </summary>
		public AsymmetricCipher Cipher
		{
			get;
			set;
		}

		public AsymmetricCipherContainer()
		{
			this.Info = new CipherInfo();
			this.Cipher = new AsymmetricCipher();
		}

		public AsymmetricCipherContainer(AsymmetricCipher cipher)
		{
            this.Info = new CipherInfo();
			this.Cipher = cipher;
		}

		public AsymmetricCipherContainer(AsymmetricCipher cipher, DateTime generateTime)
		{
			this.Info = new CipherInfo(generateTime);
			this.Cipher = cipher;
		}

        /// <summary>
        /// 序列话
        /// </summary>
        /// <returns></returns>
		public byte[] ToByteArray()
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.WriteDataAndLength(this.Info);
					binaryWriter.WriteDataAndLength(this.Cipher);
					result = memoryStream.ToArray();
				}
			}
			return result;
		}

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
		public void FromByteArray(byte[] data)
		{
			using (MemoryStream memoryStream = new MemoryStream(data))
			{
				using (BinaryReader binaryReader = new BinaryReader(memoryStream))
				{
					this.Info.FromByteArray(binaryReader.ReadBytesWithLength());
					this.Cipher.FromByteArray(binaryReader.ReadBytesWithLength());
				}
			}
		}
	}
}
