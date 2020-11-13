using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Utility.Utils
{
    /// <summary>
    /// 正在表达式匹配
    /// </summary>
    public static class RegexUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static IEnumerable<string> RegexValue(string value, string pattern)
        {
            var mc= Regex.Matches(value, pattern, RegexOptions.IgnoreCase);
            var  result=mc.Select(x=>x.Value);
            return result;
        }
        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>       
        public static bool IsMatch(string input, string pattern)
        {
            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }
    }
}
