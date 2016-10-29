using System;
using System.Security.Cryptography;

namespace CipherManager.Core
{
	public class BigPrimeGenerator : SymmetricCipherGenerator
	{
		public BigPrimeGenerator() : base(192, 8192, 8)
		{
		}

		public override SymmetricCipher Generate(int cipherSize)
		{
			if (!base.CipherSize.Check(cipherSize))
			{
				throw new ArgumentException();
			}
			byte[] content;
			using (RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider(cipherSize * 2))
			{
				RSAParameters rSAParameters = rsaCryptoServiceProvider.ExportParameters(true);
				if (new Random().Next(1) == 0)
				{
					content = rSAParameters.P;
				}
				else
				{
					content = rSAParameters.Q;
				}
			}
			return new SymmetricCipher(content);
		}
	}
}
