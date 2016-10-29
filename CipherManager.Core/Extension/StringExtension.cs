using System;
using System.Text;

namespace CipherManager.Core
{
	public static class StringExtension
	{
		public static byte[] Decode(this string s, string encodingName = "Unicode")
		{
			s = (s ?? "");
			return Encoding.GetEncoding(encodingName).GetBytes(s);
		}
	}
}
