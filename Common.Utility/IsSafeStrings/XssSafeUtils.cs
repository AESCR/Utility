using System;
using System.Text.RegularExpressions;
using Ganss.XSS;

namespace Common.Utility.IsSafeStrings
{
    /// <summary>
    /// HtmlSanitizer  XSS过滤
    /// </summary>
    public class XssSafeUtils
    {
        private static HtmlSanitizer _sanitizer = new HtmlSanitizer();
        /// <summary>
        /// XSS过滤 忽略html代码
        /// </summary>
        /// <param name="html"> html代码 </param>
        /// <returns> 过滤结果 </returns>
        public static string XssFilter(string html)
        {
            if (!string.IsNullOrWhiteSpace(html) && _sanitizer != null)
                html = _sanitizer.Sanitize(html);
            return html;
        }
    }
}