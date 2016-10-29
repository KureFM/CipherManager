using System;
using System.IO;

namespace CipherManager.Core
{
	public class SymmetricCipherContainer : IConvertibleWithByte
	{
		public CipherInfo Info
		{
			get;
			private set;
		}

		public SymmetricCipher Cipher
		{
			get;
			set;
		}

		public SymmetricCipherContainer()
		{
			this.Info = new CipherInfo();
			this.Cipher = new SymmetricCipher();
		}

		public SymmetricCipherContainer(SymmetricCipher cipher)
		{
			this.Cipher = cipher;
		}

		public SymmetricCipherContainer(SymmetricCipher cipher, DateTime generateTime)
		{
			this.Info = new CipherInfo(generateTime);
			this.Cipher = cipher;
		}

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
