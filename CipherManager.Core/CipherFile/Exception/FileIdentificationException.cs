using System;

namespace CipherManager.Core
{
	public class FileIdentificationException : CipherFileException
	{
		public FileIdentificationException() : base("文件标识错误，该文件不被CipherManager支持")
		{
		}
	}
}
