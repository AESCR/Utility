using System;
using System.Text.RegularExpressions;

namespace Common.Utility.IsSafeStrings
{
    public static class SqlSafeHelper
    {
        /// <summary>
        /// 检测是否存在与数据库相关的词
        /// </summary>
        /// <param name="str"> 字符串 </param>
        /// <returns> </returns>
        public static bool CheckBadWord(this string str)
        {
            string pattern = @"select|insert|delete|from|count\(|drop table|update|truncate|asc\(|mid\(|char\(|xp_cmdshell|exec   master|netlocalgroup administrators|net user|or|and";
            if (Regex.IsMatch(str, pattern, RegexOptions.IgnoreCase))
                return true;
            return false;
        }

        /// <summary>
        /// 完整过滤SQL字符。
        /// </summary>
        /// <param name="str"> 要过滤SQL字符的字符串。 </param>
        /// <returns> 已过滤掉SQL字符的字符串。 </returns>
        public static string GetCompleteSafeSQL(string str)
        {
            if (str == String.Empty)
                return String.Empty;
            str = str.Replace("'", "‘");//单引号替换成两个单引号
            str = str.Replace(";", "；");//半角封号替换为全角封号，防止多语句执行
            str = str.Replace(",", ",");//半角括号替换为全角括号
            str = str.Replace("?", "?");
            str = str.Replace("<", "＜");
            str = str.Replace(">", "＞");
            str = str.Replace("(", "(");
            str = str.Replace(")", ")");
            str = str.Replace("@", "＠");
            str = str.Replace("=", "＝");
            str = str.Replace("+", "＋");
            str = str.Replace("*", "＊");
            str = str.Replace("&", "＆");
            str = str.Replace("#", "＃");
            str = str.Replace("%", "％");
            str = str.Replace("$", "￥");

            //去除执行存储过程的命令关键字
            str = Regex.Replace(str, "Exec", string.Empty, RegexOptions.IgnoreCase);//不区分大小写
            str = Regex.Replace(str, "Execute", string.Empty, RegexOptions.IgnoreCase);

            //去除系统存储过程或扩展存储过程关键字
            str = Regex.Replace(str, "xp_", "x p_", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "sp_", "s p_", RegexOptions.IgnoreCase);

            //防止16进制注入
            str = Regex.Replace(str, "0x", "0 x", RegexOptions.IgnoreCase);

            //使用正则移除与数据库所有相关的关键词
            return str.ReplaceBadWord();
        }

        /// <summary>
        /// 简单过滤SQL非法字符串
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static string GetSimpleSafeSQL(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            str = Regex.Replace(str, @";", string.Empty);
            str = Regex.Replace(str, @"'", string.Empty);
            str = Regex.Replace(str, @"&", string.Empty);
            str = Regex.Replace(str, @"%20", string.Empty);
            str = Regex.Replace(str, @"--", string.Empty);
            str = Regex.Replace(str, @"==", string.Empty);
            str = Regex.Replace(str, @"<", string.Empty);
            str = Regex.Replace(str, @">", string.Empty);
            str = Regex.Replace(str, @"%", string.Empty);
            return str;
        }

        /// <summary>
        /// 检测客户输入的字符串是否有效,并将原始字符串修改为有效字符串或空字符串,并且修改原变量的值 当检测到客户的输入中有攻击性危险字符串,则返回false,有效返回true
        /// </summary>
        /// <param name="str"> 要检测的字符串 </param>
        /// <param name="IsClear"> 是否进行对传入的字符串清空处理,默认为false,置换Sql危险字符 </param>
        public static bool IsValidSafeSQL(ref string str, bool IsClear = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    //如果是空值,则跳出
                    return true;
                }
                //替换单引号
                str = str.Replace("'", "''").Trim();
                if (str.CheckBadWord())//检测到攻击字符串,开始替换危险字符串为Empty
                {
                    if (IsClear)
                        ReplaceBadWord(ref str);//置换原变量的危险SQL字符串
                    else
                        str = string.Empty;//开启清空处理,则吧原字符串置为Empty
                    return false;
                }
                //未检测到攻击字符串
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 正则替换掉与数据库相关的词
        /// </summary>
        /// <param name="str"> </param>
        /// <returns> </returns>
        public static string ReplaceBadWord(ref string str)
        {
            string[] pattern = { "select", "insert", "delete", "from", "count\\(", "drop table", "update", "truncate", "asc\\(", "mid\\(", "char\\(", "xp_cmdshell", "exec   master", "netlocalgroup administrators", "net user", "or", "and" };
            for (int i = 0; i < pattern.Length; i++)
            {
                str = Regex.Replace(str, pattern[i].ToString(), string.Empty, RegexOptions.IgnoreCase);
            }
            return str;
        }

        /// <summary>
        /// 正则替换掉与数据库相关的词
        /// </summary>
        /// <param name="str"> </param>
        /// <returns> </returns>
        public static string ReplaceBadWord(this string str)
        {
            _ = ReplaceBadWord(ref str);
            return str;
        }
    }
}