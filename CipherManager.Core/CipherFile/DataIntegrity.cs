using System;

namespace CipherManager.Core
{
	public struct DataIntegrity
	{
		public bool DataA;

		public bool DataB;

		public override string ToString()
		{
			return string.Format("DataA: {0}, DataB: {1}", this.DataA, this.DataB);
		}
	}
}
