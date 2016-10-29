using System;
using System.IO;

namespace CipherManager.Core
{
	public class CipherInfo : IConvertibleWithByte
	{
		public string Name
		{
			get;
			set;
		}

		public string Type
		{
			get;
			set;
		}

		public DateTime GenerateTime
		{
			get;
			protected set;
		}

		public int Length
		{
			get;
			protected set;
		}

		public byte[] CheckSum
		{
			get;
			set;
		}

		public string Use
		{
			get;
			set;
		}

		public string Remark
		{
			get;
			set;
		}

		public CipherInfo(DateTime generateTime)
		{
			DateTime now = DateTime.Now;
			if (generateTime > now)
			{
				throw new ArgumentException(string.Format("不允许创建一个未来的密钥，generateTime: {0} 必须在 {1} 之前", generateTime, now));
			}
			this.GenerateTime = generateTime;
			this.CheckSum = new byte[0];
		}

		public CipherInfo()
		{
			this.GenerateTime = DateTime.Now;
			this.CheckSum = new byte[0];
		}

		public virtual byte[] ToByteArray()
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.WriteDataAndLength(this.Name, "Unicode");
					binaryWriter.WriteDataAndLength(this.Type, "Unicode");
					binaryWriter.Write(this.GenerateTime);
					binaryWriter.Write(this.Length);
					binaryWriter.WriteDataAndLength(this.CheckSum);
					binaryWriter.WriteDataAndLength(this.Use, "Unicode");
					binaryWriter.WriteDataAndLength(this.Remark, "Unicode");
					result = memoryStream.ToArray();
				}
			}
			return result;
		}

		public virtual void FromByteArray(byte[] data)
		{
			using (MemoryStream memoryStream = new MemoryStream(data))
			{
				using (BinaryReader binaryReader = new BinaryReader(memoryStream))
				{
					this.Name = binaryReader.ReadStringWithLength("Unicode");
					this.Type = binaryReader.ReadStringWithLength("Unicode");
					this.GenerateTime = binaryReader.ReadDateTime();
					this.Length = binaryReader.ReadInt32();
					this.CheckSum = binaryReader.ReadBytesWithLength();
					this.Use = binaryReader.ReadStringWithLength("Unicode");
					this.Remark = binaryReader.ReadStringWithLength("Unicode");
				}
			}
		}

		public override int GetHashCode()
		{
			return this.GenerateTime.GetHashCode();
		}
	}
}
