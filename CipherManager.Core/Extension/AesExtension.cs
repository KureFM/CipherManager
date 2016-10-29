using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CipherManager.Core
{
    /// <summary>
    /// 提供一系列简化AES使用的方法
    /// </summary>
    public static class AesExtension
    {
        /// <summary>
        /// 加密byte[]
        /// </summary>
        /// <param name="aes"></param>
        /// <param name="plainText">要加密的内容</param>
        /// <returns>加密后的内容</returns>
        public static byte[] Encryption(this Aes aes, byte[] plainText)
        {
            if (aes == null)
            {
                throw new ArgumentNullException("aes");
            }
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            ICryptoTransform transform = aes.CreateEncryptor();
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainText, 0, plainText.Length);
                }
                result = memoryStream.ToArray();
            }
            return result;
        }

        /// <summary>
        /// 加密string
        /// </summary>
        /// <param name="aes"></param>
        /// <param name="plainText">要加密的内容</param>
        /// <param name="encodingName">编码器的名称</param>
        /// <returns>加密后的内容</returns>
        public static byte[] Encryption(this Aes aes, string plainText, string encodingName = "Unicode")
        {
            if (aes == null)
            {
                throw new ArgumentNullException("aes");
            }
            if (string.IsNullOrWhiteSpace(plainText))
            {
                throw new ArgumentNullException("plainText");
            }
            byte[] bytes = Encoding.GetEncoding(encodingName).GetBytes(plainText);
            return aes.Encryption(bytes);
        }

        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="aes"></param>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public static byte[] Decryption(this Aes aes, byte[] cipherText)
        {
            if (aes == null)
            {
                throw new ArgumentNullException("aes");
            }
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            ICryptoTransform transform = aes.CreateDecryptor();
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(cipherText, 0, cipherText.Length);
                }
                result = memoryStream.ToArray();
            }
            return result;
        }

        /// <summary>
        /// 解密数据到string
        /// </summary>
        /// <param name="aes"></param>
        /// <param name="cipherText"></param>
        /// <param name="encodingName"></param>
        /// <returns></returns>
        public static string DecryptionToString(this Aes aes, byte[] cipherText, string encodingName = "Unicode")
        {
            byte[] bytes = aes.Decryption(cipherText);
            return Encoding.GetEncoding(encodingName).GetString(bytes);
        }
    }
}
