using System.Security.Cryptography;
using System.Text;

namespace Common.Utility
{
    /// <summary>
    /// �õ������ȫ�루��ϣ���ܣ���
    /// </summary>
    public class HashEncode
    {
        #region Public Methods

        /// <summary>
        /// �õ�һ�������ֵ
        /// </summary>
        /// <returns> </returns>
        public static string GetRandomValue()
        {
            var Seed = new System.Random();
            var RandomVaule = Seed.Next(1, int.MaxValue).ToString();
            return RandomVaule;
        }

        /// <summary>
        /// �õ������ϣ�����ַ���
        /// </summary>
        /// <returns> </returns>
        public static string GetSecurity()
        {
            var Security = HashEncoding(GetRandomValue());
            return Security;
        }

        /// <summary>
        /// ��ϣ����һ���ַ���
        /// </summary>
        /// <param name="Security"> </param>
        /// <returns> </returns>
        public static string HashEncoding(string Security)
        {
            byte[] Value;
            var Code = new UnicodeEncoding();
            var Message = Code.GetBytes(Security);
            var Arithmetic = new SHA512Managed();
            Value = Arithmetic.ComputeHash(Message);
            Security = "";
            foreach (var o in Value) Security += (int)o + "O";
            return Security;
        }

        #endregion Public Methods
    }
}