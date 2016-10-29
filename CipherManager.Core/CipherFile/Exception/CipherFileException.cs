using System;

namespace CipherManager.Core
{
	public class CipherFileException : ApplicationException
	{
		public CipherFileException()
		{
		}

		public CipherFileException(string msg) : base(msg)
		{
		}
	}
}
