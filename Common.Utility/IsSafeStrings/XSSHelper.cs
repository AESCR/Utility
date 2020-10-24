using System;
using System.Text.RegularExpressions;

namespace Common.Utility.IsSafeStrings
{
    public class XSSHelper
    {
        #region Public Methods

        /// <summary>
        /// 过滤HTML标记
        /// </summary>
        /// <param name="htmlstring"></param>
        /// <returns></returns>
        public static string HtmlFilter(string htmlstring)
        {
            string result = Regex.Replace(htmlstring, @"<[^>]*>", String.Empty);
            return result;
        }

        /// <summary>
        /// XSS过滤
        /// </summary>
        /// <param name="html">html代码</param>
        /// <returns>过滤结果</returns>
        public static string XssFilter(string html)
        {
            string str = HtmlFilter(html);
            return str;
        }

        #endregion Public Methods
    }
}