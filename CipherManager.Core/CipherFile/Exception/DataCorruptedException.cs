using System;

namespace CipherManager.Core
{
	public class DataCorruptedException : CipherFileException
	{
		public DataCorruptedException() : base("数据已损坏，文件可能被非法修改")
		{
		}
	}
}
