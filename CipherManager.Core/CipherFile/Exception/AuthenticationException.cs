using System;

namespace CipherManager.Core
{
	public class AuthenticationException : CipherFileException
	{
		public AuthenticationException() : base("文件访问尚未认证，无法访问该项")
		{
		}
	}
}
