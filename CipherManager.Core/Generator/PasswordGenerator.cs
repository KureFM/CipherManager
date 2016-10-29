using System;
using System.Security.Cryptography;
using System.Text;

namespace CipherManager.Core
{
	public sealed class PasswordGenerator
	{
		public CipherSize CipherSize
		{
			get;
			private set;
		}

		public PasswordGenerator()
		{
			this.CipherSize = new CipherSize(1, 2048, 1);
		}

public string Generate(int cipherLen, string useChar = "", string appendChar = "")
{
	if (!this.CipherSize.Check(cipherLen))
	{
		throw new ArgumentException();
	}
	if (string.IsNullOrWhiteSpace(useChar))
	{
		useChar = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklnmopqrstuvwxyz";
	}
	if (!string.IsNullOrWhiteSpace(appendChar))
	{
		useChar += appendChar;
	}
	double num = (double)useChar.Length / 256.0;
	byte[] array = new byte[cipherLen];
	StringBuilder stringBuilder = new StringBuilder();
	using (RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider())
	{
		rNGCryptoServiceProvider.GetBytes(array);
	}
	byte[] array2 = array;
	for (int i = 0; i < array2.Length; i++)
	{
		byte b = array2[i];
		char value = useChar[(int)((double)b * num)];
		stringBuilder.Append(value);
	}
	return stringBuilder.ToString();
}
	}
}
