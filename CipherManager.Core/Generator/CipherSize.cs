using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CipherManager.Core
{
	public class CipherSize : IEnumerable<int>, IEnumerable
	{
		public int MinSize
		{
			get;
			private set;
		}

		public int MaxSize
		{
			get;
			private set;
		}

		public int Skip
		{
			get;
			private set;
		}

		public int this[int i]
		{
			get
			{
				int num = this.MinSize + this.Skip * i;
				if (!this.Check(num))
				{
					throw new ArgumentOutOfRangeException("i");
				}
				return num;
			}
		}

		public CipherSize(int minSize, int maxSize, int skip = 1)
		{
			if (maxSize < minSize)
			{
				throw new ArgumentException();
			}
			if (skip <= 0)
			{
				throw new ArgumentException();
			}
			this.MinSize = minSize;
			this.MaxSize = maxSize;
			this.Skip = skip;
		}

		public IEnumerable<int> SizeArray()
		{
			foreach (int current in Enumerable.Range(0, (int)Math.Floor((double)(this.MaxSize - this.MinSize) / (double)this.Skip) + 1))
			{
				yield return this.MinSize + current * this.Skip;
			}
			yield break;
		}

		public bool Check(int size)
		{
			int num = size - this.MinSize;
			return num >= 0 && num % this.Skip == 0;
		}

		public IEnumerator<int> GetEnumerator()
		{
			return this.SizeArray().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public bool Equals(CipherSize other)
		{
			return this.MinSize == other.MinSize && this.MaxSize == other.MaxSize && this.Skip == other.Skip;
		}

		public override bool Equals(object obj)
		{
			return obj is CipherSize && this.Equals((CipherSize)obj);
		}

		public static bool operator ==(CipherSize cs1, CipherSize cs2)
		{
			return object.Equals(cs1, cs2);
		}

		public static bool operator !=(CipherSize cs1, CipherSize cs2)
		{
			return !object.Equals(cs1, cs2);
		}

		public override int GetHashCode()
		{
			return this.MinSize.GetHashCode() + this.MaxSize.GetHashCode() + this.Skip.GetHashCode();
		}
	}
}
