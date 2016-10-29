using System;
using System.IO;

namespace CipherManager.Core
{
	public class AsymmetricCipher : IConvertibleWithByte
	{
		public CipherObject PrivateKey
		{
			get;
			private set;
		}

		public CipherObject PublicKey
		{
			get;
			private set;
		}

		public AsymmetricCipher()
		{
			this.PrivateKey = new CipherObject();
			this.PublicKey = new CipherObject();
		}

		public AsymmetricCipher(CipherObject privateKey, CipherObject publicKey)
		{
			this.PrivateKey = privateKey;
			this.PublicKey = publicKey;
		}

		public AsymmetricCipher(byte[] privateKey, byte[] publicKey) : this(new CipherObject(privateKey), new CipherObject(publicKey))
		{
		}

		public override int GetHashCode()
		{
			return this.PrivateKey.GetHashCode() + this.PublicKey.GetHashCode();
		}

		public bool Equals(AsymmetricCipher other)
		{
			return this.PrivateKey == other.PrivateKey && this.PublicKey == other.PublicKey;
		}

		public override bool Equals(object obj)
		{
			return obj is AsymmetricCipher && this.Equals((AsymmetricCipher)obj);
		}

		public static bool operator ==(AsymmetricCipher first, AsymmetricCipher second)
		{
			return object.Equals(first, second);
		}

		public static bool operator !=(AsymmetricCipher first, AsymmetricCipher second)
		{
			return !object.Equals(first, second);
		}

		public byte[] ToByteArray()
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.WriteDataAndLength(this.PrivateKey);
					binaryWriter.WriteDataAndLength(this.PublicKey);
					result = memoryStream.ToArray();
				}
			}
			return result;
		}

		public void FromByteArray(byte[] data)
		{
			using (MemoryStream memoryStream = new MemoryStream(data))
			{
				using (BinaryReader binaryReader = new BinaryReader(memoryStream))
				{
					this.PrivateKey.FromByteArray(binaryReader.ReadBytesWithLength());
					this.PublicKey.FromByteArray(binaryReader.ReadBytesWithLength());
				}
			}
		}
	}
}
