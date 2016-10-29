using System;
using System.Security.Cryptography;

namespace CipherManager.Core
{
    public class RandomNumberGenerator : SymmetricCipherGenerator
    {
        public RandomNumberGenerator() : base(8, 1048576, 8)
        {
        }

        public override SymmetricCipher Generate(int cipherSize)
        {
            if (!base.CipherSize.Check(cipherSize))
            {
                throw new ArgumentException("cipherSize");
            }
            int num = cipherSize / 8;
            byte[] array = new byte[num];
            using (RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                rngCryptoServiceProvider.GetNonZeroBytes(array);
            }
            return new SymmetricCipher(array);
        }
    }
}
