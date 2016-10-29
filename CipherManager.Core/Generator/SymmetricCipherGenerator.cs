using System;

namespace CipherManager.Core
{
    public abstract class SymmetricCipherGenerator
    {
        private static SymmetricCipherGenerator bigPrime;

        private static SymmetricCipherGenerator csrng;

        public CipherSize CipherSize
        {
            get;
            protected set;
        }

        public static SymmetricCipherGenerator BigPrime
        {
            get
            {
                return bigPrime ?? (bigPrime = new BigPrimeGenerator());
            }
        }

        public static SymmetricCipherGenerator CSRNG
        {
            get
            {
                return csrng ?? (csrng = new RandomNumberGenerator());
            }
        }

        public SymmetricCipherGenerator(int minSize, int maxSize, int skip = 8)
        {
            this.CipherSize = new CipherSize(minSize, maxSize, skip);
        }

        public abstract SymmetricCipher Generate(int cipherSize);

        public static SymmetricCipherGenerator GetGenerator(string generatorName)
        {
            throw new NotImplementedException();
        }
    }
}