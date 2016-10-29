using System;

namespace CipherManager.Core
{
	public class FileCorruptedException : CipherFileException
	{
		public FileCorruptedException() : base("文件校验失败，文件可能被非法修改")
		{
		}
	}
}
