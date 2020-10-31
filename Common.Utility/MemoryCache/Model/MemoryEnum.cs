using System;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utility.MemoryCache.Model
{
    /// <summary>
    /// 内存Key关键字
    /// </summary>
    public enum MemoryEnum
    {
        /// <summary>
        /// 黑名单
        /// </summary>
        BlackIps,

        /// <summary>
        /// 提示信息
        /// </summary>
        Prompt,

        /// <summary>
        /// 语言
        /// </summary>
        Language,

        /// <summary>
        /// 用户信息
        /// </summary>
        UserInfo,

        /// <summary>
        /// 访问权限
        /// </summary>
        UrlPermission,

        /// <summary>
        /// 获取左边菜单
        /// </summary>
        LeftMenu
    }

    public static class EnumExtensions
    {
        #region Private Methods

        private static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();
            foreach (byte t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }
            return sBuilder.ToString();
        }

        private static string Md5(string value)
        {
            var result = string.Empty;
            if (string.IsNullOrEmpty(value)) return result;
            using (var md5 = MD5.Create())
            {
                result = GetMd5Hash(md5, value);
            }
            return result;
        }

        #endregion Private Methods

        #region Public Methods

        public static string GetMemoryKey(this MemoryEnum @this)
        {
            return Md5(Enum.Parse(@this.GetType(), @this.ToString()).ToString());
        }

        #endregion Public Methods
    }
}