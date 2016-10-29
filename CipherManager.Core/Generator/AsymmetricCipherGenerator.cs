using System;

namespace CipherManager.Core
{
    public abstract class AsymmetricCipherGenerator
    {
        private static AsymmetricCipherGenerator rsa;

        private static AsymmetricCipherGenerator dsa;

        public CipherSize CipherSize
        {
            get;
            protected set;
        }

        public static AsymmetricCipherGenerator RSA
        {
            get
            {
                return rsa ?? (rsa = new RSACipherGenerator());
            }
        }

        public static AsymmetricCipherGenerator DSA
        {
            get
            {
                return dsa ?? (dsa = new DSACipherGenerator());
            }
        }

        public AsymmetricCipherGenerator(int minSize, int maxSize, int skip = 8)
        {
            this.CipherSize = new CipherSize(minSize, maxSize, skip);
        }

        public abstract AsymmetricCipher Generate(int cipherSize);

        public static AsymmetricCipherGenerator GetGenerator(string generatorName)
        {
            throw new NotImplementedException();
        }
    }
}
