using System;
using System.IO;

namespace CipherManager.Core
{
    /// <summary>
    /// �ǶԳ���Կ��Կ����
    /// </summary>
	public class AsymmetricCipherContainer : IConvertibleWithByte
	{
        /// <summary>
        /// ��Կ��Ϣ
        /// </summary>
		public CipherInfo Info
		{
			get;
			private set;
		}

        /// <summary>
        /// ��Կ����
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
        /// ���л�
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
        /// �����л�
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
