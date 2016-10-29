using System;

namespace CipherManager.Core
{
	public class DataIdentificationException : CipherFileException
	{
		public DataIdentificationException(string dataName) : base(string.Format("无法找到数据域 {0}，文件可能已被损坏", dataName))
		{
		}
	}
}
