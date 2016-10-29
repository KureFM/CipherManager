using System;
using System.Text;

namespace CipherManager.Core
{
	public static class ByteArrayExtension
	{
		public static string Encode(this byte[] bytes, string encodingName = "Unicode")
		{
			return Encoding.GetEncoding(encodingName).GetString(bytes);
		}
	}
}
