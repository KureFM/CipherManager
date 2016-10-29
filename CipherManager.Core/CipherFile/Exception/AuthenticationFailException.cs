using System;

namespace CipherManager.Core
{
	public class AuthenticationFailException : CipherFileException
	{
		public AuthenticationFailException() : base("授权验证失败")
		{
		}
	}
}
