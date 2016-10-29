using System;
using System.Security.Cryptography;

namespace CipherManager.Core
{
	public class DSACipherGenerator : AsymmetricCipherGenerator
	{
		public DSACipherGenerator() : base(512, 1024, 64)
		{
		}

public override AsymmetricCipher Generate(int cipherSize)
{
	if (!base.CipherSize.Check(cipherSize))
	{
		throw new ArgumentException();
	}
	AsymmetricCipher result;
	using (DSACryptoServiceProvider dsaCryptoServiceProvider = new DSACryptoServiceProvider(cipherSize))
	{
        // ����������ɵĹ�Կ��
		result = new AsymmetricCipher(dsaCryptoServiceProvider.ExportCspBlob(true), dsaCryptoServiceProvider.ExportCspBlob(false));
	}
	return result;
}
	}
}
