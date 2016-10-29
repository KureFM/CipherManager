using System;

namespace CipherManager.Core
{
    public class SymmetricCipher : CipherObject
    {
        public SymmetricCipher()
        {
        }

        public SymmetricCipher(byte[] content) : base(content)
        {
        }
    }
}
