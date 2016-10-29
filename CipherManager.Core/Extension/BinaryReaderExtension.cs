using System;
using System.IO;

namespace CipherManager.Core
{
	public static class BinaryReaderExtension
	{
		public static byte[] ReadBytesWithLength(this BinaryReader br)
		{
			if (!br.BaseStream.CanRead)
			{
				throw new ArgumentException("br不可读");
			}
			int count = br.ReadInt32();
			return br.ReadBytes(count);
		}

		public static string ReadStringWithLength(this BinaryReader br, string encodingName = "Unicode")
		{
			return br.ReadBytesWithLength().Encode(encodingName);
		}

		public static DateTime ReadDateTime(this BinaryReader br)
		{
			return new DateTime(br.ReadInt64());
		}

		public static Guid ReadGuid(this BinaryReader br)
		{
			return new Guid(br.ReadBytes(16));
		}
	}
}
