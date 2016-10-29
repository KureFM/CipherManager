using System;

namespace CipherManager.Core
{
    /// <summary>
    /// 定义密钥对象序列化和反序列化的接口
    /// </summary>
    public interface IConvertibleWithByte
    {
        /// <summary>
        /// 序列化：将对象转化为byte[]
        /// </summary>
        /// <returns>序列化后的数据</returns>
        byte[] ToByteArray();

        /// <summary>
        /// 反序列化：将byte[]转化为对象
        /// </summary>
        /// <param name="data">将要反序列化的数据</param>
        void FromByteArray(byte[] data);
    }
}
