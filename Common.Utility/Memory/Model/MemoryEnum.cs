using System;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utility.Memory.Model
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
        LeftMenu,

        /// <summary>
        /// UserAgent
        /// </summary>
        UserAgent,

        Proxy,
    }

    public static class EnumExtensions
    {
        public static string GetMemoryKey(this MemoryEnum @this, string value = null)
        {
            string enumName = Enum.Parse(@this.GetType(), @this.ToString()).ToString();
            if (string.IsNullOrWhiteSpace(enumName) != false) throw new ArgumentNullException(nameof(MemoryEnum));
            using var md5 = MD5.Create();
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(enumName));
            var sBuilder = new StringBuilder();
            foreach (byte t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                return "System" + sBuilder.Append(enumName);
            }
            return "System" + sBuilder.Append(enumName).Append(value);
        }
    }
}