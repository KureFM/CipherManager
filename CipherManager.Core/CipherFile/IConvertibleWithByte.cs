using System;

namespace CipherManager.Core
{
    /// <summary>
    /// ������Կ�������л��ͷ����л��Ľӿ�
    /// </summary>
    public interface IConvertibleWithByte
    {
        /// <summary>
        /// ���л���������ת��Ϊbyte[]
        /// </summary>
        /// <returns>���л��������</returns>
        byte[] ToByteArray();

        /// <summary>
        /// �����л�����byte[]ת��Ϊ����
        /// </summary>
        /// <param name="data">��Ҫ�����л�������</param>
        void FromByteArray(byte[] data);
    }
}
