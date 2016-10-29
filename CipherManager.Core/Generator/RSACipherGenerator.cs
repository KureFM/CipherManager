using System;
using System.Security.Cryptography;

namespace CipherManager.Core
{
    public class RSACipherGenerator : AsymmetricCipherGenerator
    {
        public RSACipherGenerator() : base(384, 16384, 8)
        {
        }

        public override AsymmetricCipher Generate(int cipherSize)
        {
            if (!base.CipherSize.Check(cipherSize))
            {
                throw new ArgumentException(nameof(cipherSize));
            }
            AsymmetricCipher result;
            using (RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider(cipherSize))
            {
                result = new AsymmetricCipher(rsaCryptoServiceProvider.ExportCspBlob(true), rsaCryptoServiceProvider.ExportCspBlob(false));
            }
            return result;
        }
    }
}
