using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utility
{
    /// <summary>
    /// ���ܽ���ʵ���ࡣ
    /// </summary>
    public class Encrypt
    {
        #region Private Fields

        private static readonly byte[] arrDESIV = { 55, 103, 246, 79, 36, 99, 167, 3 };

        //��Կ
        private static readonly byte[] arrDESKey = { 42, 16, 93, 156, 78, 4, 218, 32 };

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// ���ܡ�
        /// </summary>
        /// <param name="m_Need_Encode_String"> </param>
        /// <returns> </returns>
        public static string Decode(string m_Need_Encode_String)
        {
            if (m_Need_Encode_String == null) throw new Exception("Error: \nԴ�ַ���Ϊ�գ���");
            var objDES = new DESCryptoServiceProvider();
            var arrInput = Convert.FromBase64String(m_Need_Encode_String);
            var objMemoryStream = new MemoryStream(arrInput);
            var objCryptoStream = new CryptoStream(objMemoryStream, objDES.CreateDecryptor(arrDESKey, arrDESIV),
                CryptoStreamMode.Read);
            var objStreamReader = new StreamReader(objCryptoStream);
            return objStreamReader.ReadToEnd();
        }

        /// <summary>
        /// ���ܡ�
        /// </summary>
        /// <param name="m_Need_Encode_String"> </param>
        /// <returns> </returns>
        public static string Encode(string m_Need_Encode_String)
        {
            if (m_Need_Encode_String == null) throw new Exception("Error: \nԴ�ַ���Ϊ�գ���");
            var objDES = new DESCryptoServiceProvider();
            var objMemoryStream = new MemoryStream();
            var objCryptoStream = new CryptoStream(objMemoryStream, objDES.CreateEncryptor(arrDESKey, arrDESIV),
                CryptoStreamMode.Write);
            var objStreamWriter = new StreamWriter(objCryptoStream);
            objStreamWriter.Write(m_Need_Encode_String);
            objStreamWriter.Flush();
            objCryptoStream.FlushFinalBlock();
            objMemoryStream.Flush();
            return Convert.ToBase64String(objMemoryStream.GetBuffer(), 0, (int)objMemoryStream.Length);
        }

        /// <summary>
        /// 32λMD5����
        /// </summary>
        /// <param name="strText"> Ҫ�����ַ��� </param>
        /// <param name="IsLower"> �Ƿ���Сд��ʽ���� </param>
        /// <returns> </returns>
        public static string MD5Encrypt(string strText, bool IsLower)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var bytes = Encoding.UTF8.GetBytes(strText);
            bytes = md5.ComputeHash(bytes);
            md5.Clear();

            var ret = "";
            for (var i = 0; i < bytes.Length; i++) ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');

            return ret.PadLeft(32, '0');
        }

        #endregion Public Methods
    }
}