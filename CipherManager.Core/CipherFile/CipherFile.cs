using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace CipherManager.Core
{
    public class CipherFile
    {
        #region 常量

        private const int CIPHER_LENGTH = 256;

        private const int IV_LENGTH = 128;

        private const int SALT_BYTE_SIZE = 32;

        private const int HASH_BYTE_SIZE = 32;

        private const int IV_BYTE_SIZE = 16;

        private const int PBKDF2_ITERATIONS = 1000;

        private const PaddingMode PADDING_MODE = PaddingMode.PKCS7;

        private const CipherMode CIPHER_MODE = CipherMode.OFB;

        private char[] FILE_IDENTIFICATION = "!CIPHERMANAGER".ToCharArray();

        private char[] DATA_A_IDENTIFICATION = "DATA1".ToCharArray();

        private char[] DATA_B_IDENTIFICATION = "DATA2".ToCharArray();

        #endregion

        #region 加密服务提供对象

        private SymmetricCipherGenerator csrng = SymmetricCipherGenerator.CSRNG;

        private Aes aesForDataA;

        private Aes aesForDataB;

        private HMACSHA256 hmacsha256;

        #endregion

        #region 私有变量

        private string fileName;

        private string email;

        private FileStream fileStream;

        private bool authentication;

        private Guid guid;

        private long headerLength;

        private byte[] encryptedEmail;

        private byte[] hashedPassword;

        private byte[] saltForPassword;

        private byte[] saltForDataA;

        private byte[] saltForDataAIV;

        private byte[] saltForDataB;

        private byte[] saltForDataBIV;

        private long dataLength;

        private List<AsymmetricCipherContainer> asymmetricCipherList = new List<AsymmetricCipherContainer>();

        private List<SymmetricCipherContainer> symmetricCipherList = new List<SymmetricCipherContainer>();

        private List<DailyCipherContainer> dailyCipherList = new List<DailyCipherContainer>();

        #endregion

        #region 公开属性

        public bool Authentication
        {
            get
            {
                return this.authentication;
            }
        }

        public Guid Guid
        {
            get
            {
                return this.guid;
            }
        }

        public string Email
        {
            get
            {
                if (this.Authentication)
                {
                    return this.email;
                }
                throw new AuthenticationException();
            }
        }

        public string FileName
        {
            get
            {
                return this.fileName;
            }
            set
            {
                if (!this.Authentication)
                {
                    throw new AuthenticationException();
                }
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException();
                }
                this.CreateOrModifyFileName(value);
            }
        }

        public DateTime CreationTime
        {
            get
            {
                return File.GetCreationTime(this.FileName);
            }
        }

        public DateTime LastModifyTime
        {
            get
            {
                return File.GetLastWriteTime(this.FileName);
            }
        }

        public List<AsymmetricCipherContainer> AsymmetricCipherList
        {
            get
            {
                if (this.Authentication)
                {
                    return this.asymmetricCipherList;
                }
                throw new AuthenticationException();
            }
        }

        public List<SymmetricCipherContainer> SymmetricCipherList
        {
            get
            {
                if (this.Authentication)
                {
                    return this.symmetricCipherList;
                }
                throw new AuthenticationException();
            }
        }

        public List<DailyCipherContainer> DailyCipherList
        {
            get
            {
                if (this.Authentication)
                {
                    return this.dailyCipherList;
                }
                throw new AuthenticationException();
            }
        }

        #endregion

        private void CreateOrModifyGuid()
        {
            this.guid = Guid.NewGuid();
        }

        private void CreatePassword(string password)
        {
            if (this.hashedPassword != null)
            {
                throw new ApplicationException("Password已经存在，若要修改密码请使用ModifyPassword");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("password");
            }
            this.saltForPassword = this.csrng.Generate(256).Bytes;
            this.hashedPassword = CipherFile.PBKDF2(password, this.saltForPassword, 1000, 32);
            this.CreateCryptProvider(password);
        }

        private void CreateCryptProvider(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("password");
            }
            if (this.saltForDataA == null)
            {
                this.saltForDataA = this.csrng.Generate(256).Bytes;
            }
            if (this.saltForDataAIV == null)
            {
                this.saltForDataAIV = this.csrng.Generate(128).Bytes;
            }
            if (this.saltForDataB == null || this.saltForDataBIV == null)
            {
                this.saltForDataB = this.csrng.Generate(256).Bytes;
            }
            if (this.saltForDataBIV == null)
            {
                this.saltForDataBIV = this.csrng.Generate(128).Bytes;
            }
            byte[] array = CipherFile.PBKDF2(password, this.saltForDataA, 1000, 32);
            byte[] iv = CipherFile.PBKDF2(password, this.saltForDataAIV, 1000, 16);
            byte[] array2 = CipherFile.PBKDF2(password, this.saltForDataB, 1000, 32);
            byte[] iv2 = CipherFile.PBKDF2(password, this.saltForDataBIV, 1000, 16);
            byte[] key = array.Concat(array2).ToArray<byte>();
            this.aesForDataA = this.CreateAesProvider(array, iv);
            this.aesForDataB = this.CreateAesProvider(array2, iv2);
            this.hmacsha256 = new HMACSHA256(key);
        }

        private void CreateAesForDataA(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("password");
            }
            if (this.saltForDataA == null)
            {
                this.saltForDataA = this.csrng.Generate(256).Bytes;
            }
            if (this.saltForDataAIV == null)
            {
                this.saltForDataAIV = this.csrng.Generate(128).Bytes;
            }
            this.aesForDataA = this.CreateAesProvider(CipherFile.PBKDF2(password, this.saltForDataA, 1000, 32), CipherFile.PBKDF2(password, this.saltForDataAIV, 1000, 16));
        }

        private void CreateCryptProviderWithoutAesForDataA(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("password");
            }
            if (this.saltForDataA == null)
            {
                this.saltForDataA = this.csrng.Generate(256).Bytes;
            }
            if (this.saltForDataB == null || this.saltForDataBIV == null)
            {
                this.saltForDataB = this.csrng.Generate(256).Bytes;
            }
            if (this.saltForDataBIV == null)
            {
                this.saltForDataBIV = this.csrng.Generate(128).Bytes;
            }
            byte[] first = CipherFile.PBKDF2(password, this.saltForDataA, 1000, 32);
            byte[] array = CipherFile.PBKDF2(password, this.saltForDataB, 1000, 32);
            byte[] key = first.Concat(array).ToArray<byte>();
            this.aesForDataB = this.CreateAesProvider(array, CipherFile.PBKDF2(password, this.saltForDataBIV, 1000, 16));
            this.hmacsha256 = new HMACSHA256(key);
        }

        private void ModifyPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("password");
            }
            this.saltForPassword = this.csrng.Generate(256).Bytes;
            this.hashedPassword = CipherFile.PBKDF2(password, this.saltForPassword, 1000, 32);
            this.saltForDataA = this.csrng.Generate(256).Bytes;
            this.saltForDataBIV = this.csrng.Generate(128).Bytes;
            this.saltForDataB = this.csrng.Generate(256).Bytes;
            this.saltForDataBIV = this.csrng.Generate(128).Bytes;
            this.CreateCryptProvider(password);
        }

        private Aes CreateAesProvider(byte[] key, byte[] iv)
        {
            Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Padding = PADDING_MODE;
            aes.Mode = CIPHER_MODE;
            return aes;
        }

        private void CreateOrModifyEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException("email");
            }
            if (this.aesForDataA == null)
            {
                throw new CipherFileException("aesForDataA 尚未初始化");
            }
            this.email = email;
            this.encryptedEmail = this.aesForDataA.Encryption(email, "Unicode");
        }

        private void CreateOrModifyFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName");
            }
            if (this.fileName == fileName)
            {
                throw new CipherFileException(string.Format("名为 {0} 的文件已经在对象中打开", fileName));
            }
            this.fileStream = this.CreateFileStream(fileName);
            this.fileName = this.fileStream.Name;
        }

        private FileStream CreateFileStream(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName");
            }
            return new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        }

        private byte[] CombineData()
        {
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(this.asymmetricCipherList.Count);
                    foreach (AsymmetricCipherContainer current in this.asymmetricCipherList)
                    {
                        binaryWriter.WriteDataAndLength(current);
                    }
                    binaryWriter.Write(this.symmetricCipherList.Count);
                    foreach (SymmetricCipherContainer current2 in this.symmetricCipherList)
                    {
                        binaryWriter.WriteDataAndLength(current2);
                    }
                    binaryWriter.Write(this.dailyCipherList.Count);
                    foreach (DailyCipherContainer current3 in this.dailyCipherList)
                    {
                        binaryWriter.WriteDataAndLength(current3);
                    }
                    result = memoryStream.ToArray();
                }
            }
            return result;
        }

        private void SplitData(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                {
                    int count = binaryReader.ReadInt32();
                    foreach (int i in Enumerable.Range(1, count))
                    {
                        AsymmetricCipherContainer asymmetricCipherContainer = new AsymmetricCipherContainer();
                        asymmetricCipherContainer.FromByteArray(binaryReader.ReadBytesWithLength());
                        this.asymmetricCipherList.Add(asymmetricCipherContainer);
                    }
                    count = binaryReader.ReadInt32();
                    foreach (int i in Enumerable.Range(1, count))
                    {
                        SymmetricCipherContainer symmetricCipherContainer = new SymmetricCipherContainer();
                        symmetricCipherContainer.FromByteArray(binaryReader.ReadBytesWithLength());
                        this.symmetricCipherList.Add(symmetricCipherContainer);
                    }
                    count = binaryReader.ReadInt32();
                    foreach (int i in Enumerable.Range(1, count))
                    {
                        DailyCipherContainer dailyCipherContainer = new DailyCipherContainer();
                        dailyCipherContainer.FromByteArray(binaryReader.ReadBytesWithLength());
                        this.dailyCipherList.Add(dailyCipherContainer);
                    }
                }
            }
        }

        private byte[] ComputeDataHash(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            return this.hmacsha256.ComputeHash(data);
        }

        private void WriteFileHeader(FileStream fs = null)
        {
            if (fs == null)
            {
                if (this.fileStream == null)
                {
                    throw new CipherFileException("fileStream 尚未初始化");
                }
                fs = this.fileStream;
            }
            fs.Seek(0L, SeekOrigin.Begin);
            BinaryWriter binaryWriter = new BinaryWriter(fs);
            binaryWriter.Write(this.FILE_IDENTIFICATION);
            binaryWriter.Write(this.guid);
            fs.Seek(8L, SeekOrigin.Current);
            binaryWriter.WriteDataAndLength(this.encryptedEmail);
            binaryWriter.Write(this.hashedPassword);
            binaryWriter.Write(this.saltForPassword);
            binaryWriter.Write(this.saltForDataA);
            binaryWriter.Write(this.saltForDataAIV);
            binaryWriter.Write(this.saltForDataB);
            binaryWriter.Write(this.saltForDataBIV);
            this.headerLength = fs.Position;
            fs.Seek(30L, SeekOrigin.Begin);
            binaryWriter.Write(this.headerLength);
            fs.SetLength(this.headerLength);
            fs.Flush();
        }

        private void WriteDataField(FileStream fs = null)
        {
            if (fs == null)
            {
                if (this.fileStream == null)
                {
                    throw new CipherFileException("fileStream 尚未初始化");
                }
                fs = this.fileStream;
            }
            if (this.headerLength != 0L)
            {
                fs.Seek(this.headerLength, SeekOrigin.Begin);
            }
            fs.Seek(8L, SeekOrigin.Current);
            BinaryWriter binaryWriter = new BinaryWriter(fs);
            binaryWriter.Write(this.DATA_A_IDENTIFICATION);
            byte[] data = this.aesForDataA.Encryption(this.CombineData());
            binaryWriter.WriteDataAndLength(data);
            binaryWriter.Write(this.ComputeDataHash(data));
            binaryWriter.Write(this.DATA_B_IDENTIFICATION);
            data = this.aesForDataB.Encryption(this.CombineData());
            binaryWriter.WriteDataAndLength(data);
            binaryWriter.Write(this.ComputeDataHash(data));
            this.dataLength = fs.Position - this.headerLength;

            fs.Seek(this.headerLength, SeekOrigin.Begin);
            binaryWriter.Write(this.dataLength);
            fs.SetLength(this.headerLength + this.dataLength);
            fs.Flush();
        }

        private void WriteChecksumField(FileStream fs = null)
        {
            if (fs == null)
            {
                if (this.fileStream == null)
                {
                    throw new CipherFileException("fileStream 尚未初始化");
                }
                fs = this.fileStream;
            }
            if (this.headerLength != 0L)
            {
                fs.Seek(this.headerLength + this.dataLength, SeekOrigin.Begin);
            }
            byte[] buffer = this.ComputeFileHash();
            BinaryWriter binaryWriter = new BinaryWriter(fs);
            binaryWriter.Write(buffer);
            fs.Flush();
        }

        private byte[] ComputeFileHash()
        {
            if (this.headerLength == 0L)
            {
                throw new CipherFileException("请先保存文件头和数据域");
            }
            if (this.hmacsha256 == null)
            {
                throw new CipherFileException("hmacsha256 尚未初始化");
            }
            long num = this.headerLength + this.dataLength;
            long num2 = this.fileStream.Length - num;
            if (num2 > 0L)
            {
                BinaryReader binaryReader = this.GetBinaryReader();
                this.fileStream.Seek(-num2, SeekOrigin.End);
                byte[] buffer = binaryReader.ReadBytes((int)num2);
                this.fileStream.SetLength(this.headerLength + this.dataLength);
                byte[] result = this.hmacsha256.ComputeHash(this.fileStream);
                BinaryWriter binaryWriter = new BinaryWriter(this.fileStream);
                this.fileStream.Seek(0L, SeekOrigin.End);
                binaryWriter.Write(buffer);
                return result;
            }
            return this.hmacsha256.ComputeHash(this.fileStream);
        }

        private void SaveToFile(string fileName = "")
        {
            FileStream fs;
            if (this.fileName != fileName && !string.IsNullOrWhiteSpace(fileName))
            {
                fs = this.CreateFileStream(fileName);
            }
            else
            {
                fs = this.fileStream;
            }
            this.WriteFileHeader(fs);
            this.WriteDataField(fs);
            this.WriteChecksumField(fs);
        }

        private void CreateFile(string email, string password, string fileName = "")
        {
            this.authentication = true;
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException("email");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("password");
            }
            this.CreateOrModifyGuid();
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = this.guid.ToString();
            }
            this.CreatePassword(password);
            this.CreateOrModifyEmail(email);
            this.CreateOrModifyFileName(fileName);
            this.SaveToFile("");
        }

        private BinaryReader GetBinaryReader()
        {
            if (this.fileStream == null)
            {
                throw new CipherFileException("fileStream 尚未初始化");
            }
            return new BinaryReader(this.fileStream);
        }

        private bool VerifyFileHeader()
        {
            BinaryReader binaryReader = this.GetBinaryReader();
            this.fileStream.Seek(0L, SeekOrigin.Begin);
            char[] first = binaryReader.ReadChars(14);
            return first.SequenceEqual(this.FILE_IDENTIFICATION);
        }

        private bool VerifyAuthentication(string email, string password)
        {
            BinaryReader binaryReader = this.GetBinaryReader();
            this.headerLength = binaryReader.ReadInt64();
            this.encryptedEmail = binaryReader.ReadBytesWithLength();
            this.hashedPassword = binaryReader.ReadBytes(32);
            this.saltForPassword = binaryReader.ReadBytes(32);
            if (!this.hashedPassword.SequenceEqual(CipherFile.PBKDF2(password, this.saltForPassword, 1000, 32)))
            {
                return false;
            }
            this.saltForDataA = binaryReader.ReadBytes(32);
            this.saltForDataAIV = binaryReader.ReadBytes(16);
            this.CreateAesForDataA(password);
            if (email != this.aesForDataA.DecryptionToString(this.encryptedEmail, "Unicode"))
            {
                return false;
            }
            this.authentication = true;
            this.saltForDataB = binaryReader.ReadBytes(32);
            this.saltForDataBIV = binaryReader.ReadBytes(16);
            this.CreateCryptProviderWithoutAesForDataA(password);
            this.email = email;
            return true;
        }

        private bool VerifyIntegrity()
        {
            BinaryReader binaryReader = this.GetBinaryReader();
            this.LoadDataLength();
            this.fileStream.Seek(-32L, SeekOrigin.End);
            byte[] first = binaryReader.ReadBytes(32);
            byte[] second = this.ComputeFileHash();
            return first.SequenceEqual(second);
        }

        private DataIntegrity VerifyData()
        {
            BinaryReader binaryReader = this.GetBinaryReader();
            this.fileStream.Seek(this.headerLength + 8L, SeekOrigin.Begin);
            char[] first = binaryReader.ReadChars(this.DATA_A_IDENTIFICATION.Length);
            if (!first.SequenceEqual(this.DATA_A_IDENTIFICATION))
            {
                throw new DataIdentificationException("A");
            }
            byte[] data = binaryReader.ReadBytesWithLength();
            byte[] first2 = binaryReader.ReadBytes(32);
            bool dataA = false;
            if (first2.SequenceEqual(this.ComputeDataHash(data)))
            {
                dataA = true;
            }
            first = binaryReader.ReadChars(this.DATA_B_IDENTIFICATION.Length);
            if (!first.SequenceEqual(this.DATA_B_IDENTIFICATION))
            {
                throw new DataIdentificationException("B");
            }
            data = binaryReader.ReadBytesWithLength();
            first2 = binaryReader.ReadBytes(32);
            bool dataB = false;
            if (first2.SequenceEqual(this.ComputeDataHash(data)))
            {
                dataB = true;
            }
            return new DataIntegrity
            {
                DataA = dataA,
                DataB = dataB
            };
        }

        private void LoadGuid()
        {
            BinaryReader binaryReader = this.GetBinaryReader();
            this.fileStream.Seek(14L, SeekOrigin.Begin);
            this.guid = binaryReader.ReadGuid();
        }

        private void LoadHeaderLength()
        {
            BinaryReader binaryReader = this.GetBinaryReader();
            this.fileStream.Seek(30L, SeekOrigin.Begin);
            this.headerLength = binaryReader.ReadInt64();
        }

        private void LoadDataLength()
        {
            BinaryReader binaryReader = this.GetBinaryReader();
            if (this.headerLength == 0L)
            {
                this.LoadHeaderLength();
            }
            this.fileStream.Seek(this.headerLength, SeekOrigin.Begin);
            this.dataLength = binaryReader.ReadInt64();
        }

        private void LoadFromFile(string fileName)
        {
            if (this.fileName == fileName)
            {
                throw new ApplicationException(string.Format("文件 {0} 已在当前对象中打开", fileName));
            }
            this.CreateOrModifyFileName(fileName);
            if (!this.VerifyFileHeader())
            {
                throw new FileIdentificationException();
            }
            this.LoadGuid();
        }

        private void LoadData()
        {
            DataIntegrity dataIntegrity = this.VerifyData();
            if (dataIntegrity.DataA)
            {
                BinaryReader binaryReader = this.GetBinaryReader();
                this.fileStream.Seek(this.headerLength + 8L + (long)this.DATA_A_IDENTIFICATION.Length, SeekOrigin.Begin);
                byte[] cipherText = binaryReader.ReadBytesWithLength();
                byte[] data = this.aesForDataA.Decryption(cipherText);
                this.SplitData(data);
                return;
            }
            if (dataIntegrity.DataB)
            {
                throw new DataCorruptedException();
            }
            throw new DataCorruptedException();
        }

        public CipherFile(string email, string password, string fileName = "")
        {
            this.CreateFile(email, password, fileName);
        }

        public CipherFile(string fileName)
        {
            this.LoadFromFile(fileName);
        }

        public bool GetAuthentication(string email, string password)
        {
            if (this.Authentication)
            {
                throw new CipherFileException("已经获得授权");
            }
            bool result = this.VerifyAuthentication(email, password);
            this.LoadData();
            return result;
        }

        public bool CheckFileIntegrity()
        {
            if (!this.Authentication)
            {
                throw new AuthenticationException();
            }
            return this.VerifyIntegrity();
        }

        public DataIntegrity CheckDataIntegrity()
        {
            if (!this.Authentication)
            {
                throw new AuthenticationException();
            }
            return this.VerifyData();
        }

        public bool CheckIdentification()
        {
            return this.VerifyFileHeader();
        }

        public void SaveData()
        {
            if (!this.Authentication)
            {
                throw new AuthenticationException();
            }
            this.WriteDataField(null);
            this.WriteChecksumField(null);
        }

        private static byte[] PBKDF2(string password, byte[] salt, int iterations = 1000, int outputBytes = 32)
        {
            byte[] bytes;
            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                bytes = rfc2898DeriveBytes.GetBytes(outputBytes);
            }
            return bytes;
        }

        ~CipherFile()
        {
            this.aesForDataA?.Clear();
            this.aesForDataB?.Clear();
            this.hmacsha256?.Clear();
            this.fileStream?.Close();
        }
    }
}