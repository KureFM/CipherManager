using System;
using System.IO;

namespace CipherManager.Core
{
	public class DailyCipherContainer : IConvertibleWithByte
	{
		public CipherInfo Info
		{
			get;
			private set;
		}

		public string UserName
		{
			get;
			set;
		}

		public string Password
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}

		public DailyCipherContainer()
		{
			this.Info = new CipherInfo();
		}

		public DailyCipherContainer(string username, string password)
		{
			this.Info = new CipherInfo();
			this.UserName = username;
			this.Password = password;
		}

		public DailyCipherContainer(string username, string password, DateTime generateTime)
		{
			this.Info = new CipherInfo(generateTime);
			this.UserName = username;
			this.Password = password;
		}

		public byte[] ToByteArray()
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.WriteDataAndLength(this.Info);
					binaryWriter.WriteDataAndLength(this.UserName, "Unicode");
					binaryWriter.WriteDataAndLength(this.Password, "Unicode");
					binaryWriter.WriteDataAndLength(this.Url, "Unicode");
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
					this.UserName = binaryReader.ReadStringWithLength("Unicode");
					this.Password = binaryReader.ReadStringWithLength("Unicode");
					this.Url = binaryReader.ReadStringWithLength("Unicode");
				}
			}
		}
	}
}
