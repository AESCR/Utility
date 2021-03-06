﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Utility.Tools
{
    /// <summary>
    /// 验证工具类
    /// </summary>
    public class ValidUtils
    {
        /// <summary>
        /// 验证日期是否合法,对不规则的作了简单处理
        /// </summary>
        /// <param name="date"> 日期 </param>
        public static bool IsDate(ref string date)
        {
            //如果为空，认为验证合格
            if (IsNullOrEmpty(date)) return true;

            //清除要验证字符串中的空格
            date = date.Trim();

            //替换\
            date = date.Replace(@"\", "-");
            //替换/
            date = date.Replace(@"/", "-");

            //如果查找到汉字"今",则认为是当前日期
            if (date.IndexOf("今") != -1) date = DateTime.Now.ToString();

            try
            {
                //用转换测试是否为规则的日期字符
                date = Convert.ToDateTime(date).ToString("d");
                return true;
            }
            catch
            {
                //如果日期字符串中存在非数字，则返回false
                if (!IsInt(date)) return false;

                //对8位纯数字进行解析
                if (date.Length == 8)
                {
                    //获取年月日
                    var year = date.Substring(0, 4);
                    var month = date.Substring(4, 2);
                    var day = date.Substring(6, 2);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100) return false;
                    if (Convert.ToInt32(month) > 12 || Convert.ToInt32(day) > 31) return false;

                    //拼接日期
                    date = Convert.ToDateTime(year + "-" + month + "-" + day).ToString("d");
                    return true;
                }

                //对6位纯数字进行解析
                if (date.Length == 6)
                {
                    //获取年月
                    var year = date.Substring(0, 4);
                    var month = date.Substring(4, 2);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100) return false;
                    if (Convert.ToInt32(month) > 12) return false;

                    //拼接日期
                    date = Convert.ToDateTime(year + "-" + month).ToString("d");
                    return true;
                }

                //对5位纯数字进行解析
                if (date.Length == 5)
                {
                    //获取年月
                    var year = date.Substring(0, 4);
                    var month = date.Substring(4, 1);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100) return false;

                    //拼接日期
                    date = year + "-" + month;
                    return true;
                }

                //对4位纯数字进行解析
                if (date.Length == 4)
                {
                    //获取年
                    var year = date.Substring(0, 4);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100) return false;

                    //拼接日期
                    date = Convert.ToDateTime(year).ToString("d");
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// 是否为Double类型
        /// </summary>
        /// <param name="expression"> </param>
        /// <returns> </returns>
        public static bool IsDouble(object expression)
        {
            if (expression != null)
                return Regex.IsMatch(expression.ToString(), @"^([0-9])[0-9]*(\.\w*)?$");

            return false;
        }

        /// <summary>
        /// 验证EMail是否合法
        /// </summary>
        /// <param name="email"> 要验证的Email </param>
        public static bool IsEmail(string email)
        {
            //如果为空，认为验证不合格
            if (IsNullOrEmpty(email)) return false;

            //清除要验证字符串中的空格
            email = email.Trim();

            //模式字符串
            var pattern = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";

            //验证
            return IsMatch(email, pattern);
        }

        /// <summary>
        /// 验证身份证是否合法
        /// </summary>
        /// <param name="idCard"> 要验证的身份证 </param>
        public static bool IsIdCard(string idCard)
        {
            //如果为空，认为验证合格
            if (IsNullOrEmpty(idCard)) return true;

            //清除要验证字符串中的空格
            idCard = idCard.Trim();

            //模式字符串
            var pattern = new StringBuilder();
            pattern.Append(@"^(11|12|13|14|15|21|22|23|31|32|33|34|35|36|37|41|42|43|44|45|46|");
            pattern.Append(@"50|51|52|53|54|61|62|63|64|65|71|81|82|91)");
            pattern.Append(@"(\d{13}|\d{15}[\dx])$");

            //验证
            return IsMatch(idCard, pattern.ToString());
        }

        /// <summary>
        /// 验证是否为整数 如果为空，认为验证不合格 返回false
        /// </summary>
        /// <param name="number"> 要验证的整数 </param>
        public static bool IsInt(string number)
        {
            //如果为空，认为验证不合格
            if (IsNullOrEmpty(number)) return false;

            //清除要验证字符串中的空格
            number = number.Trim();

            //模式字符串
            var pattern = @"^[0-9]+[0-9]*$";

            //验证
            return IsMatch(number, pattern);
        }

        /// <summary>
        /// 验证IP地址是否合法
        /// </summary>
        /// <param name="ip"> 要验证的IP地址 </param>
        public static bool IsIP(string ip)
        {
            //如果为空，认为验证合格
            if (IsNullOrEmpty(ip)) return true;

            //清除要验证字符串中的空格
            ip = ip.Trim();

            //模式字符串
            var pattern = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";

            //验证
            return IsMatch(ip, pattern);
        }

        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input"> 输入字符串 </param>
        /// <param name="pattern"> 模式字符串 </param>
        public static bool IsMatch(string input, string pattern)
        {
            return IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input"> 输入的字符串 </param>
        /// <param name="pattern"> 模式字符串 </param>
        /// <param name="options"> 筛选条件 </param>
        public static bool IsMatch(string input, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(input, pattern, options);
        }

        /// <summary>
        /// 判断对象是否为空，为空返回true
        /// </summary>
        /// <typeparam name="T"> 要验证的对象的类型 </typeparam>
        /// <param name="data"> 要验证的对象 </param>
        public static bool IsNullOrEmpty<T>(T data)
        {
            //如果为null
            if (data == null) return true;

            //如果为""
            if (data.GetType() == typeof(string))
                if (string.IsNullOrEmpty(data.ToString().Trim()))
                    return true;

            //如果为DBNull
            if (data.GetType() == typeof(DBNull)) return true;

            //不为空
            return false;
        }

        /// <summary>
        /// 判断对象是否为空，为空返回true
        /// </summary>
        /// <param name="data"> 要验证的对象 </param>
        public static bool IsNullOrEmpty(object data)
        {
            //如果为null
            if (data == null) return true;

            //如果为""
            if (data.GetType() == typeof(string))
                if (string.IsNullOrEmpty(data.ToString().Trim()))
                    return true;

            //如果为DBNull
            if (data.GetType() == typeof(DBNull)) return true;

            //不为空
            return false;
        }

        /// <summary>
        /// 验证是否为数字
        /// </summary>
        /// <param name="number"> 要验证的数字 </param>
        public static bool IsNumber(string number)
        {
            //如果为空，认为验证不合格
            if (IsNullOrEmpty(number)) return false;

            //清除要验证字符串中的空格
            number = number.Trim();

            //模式字符串
            var pattern = @"^[0-9]+[0-9]*[.]?[0-9]*$";

            //验证
            return IsMatch(number, pattern);
        }

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"> </param>
        /// <returns> </returns>
        public static bool IsNumeric(string expression)
        {
            if (expression != null)
            {
                var str = expression;
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                    if (str.Length < 10 || str.Length == 10 && str[0] == '1' ||
                        str.Length == 11 && str[0] == '-' && str[1] == '1')
                        return true;
            }

            return false;
        }

        /// <summary>
        /// 实体验证
        /// </summary>
        /// <param name="value"> </param>
        /// <param name="results"> </param>
        /// <returns> </returns>
        public static bool IsValid(object value, out List<ValidationResult> results)
        {
            results = new List<ValidationResult>();
            var validationContext = new ValidationContext(value);
            var isValid = Validator.TryValidateObject(value, validationContext, results, true);
            return isValid;
        }

        /// <summary>
        /// 检测客户输入的字符串是否有效,并将原始字符串修改为有效字符串或空字符串。 当检测到客户的输入中有攻击性危险字符串,则返回false,有效返回true。
        /// </summary>
        /// <param name="input"> 要检测的字符串 </param>
        public static bool IsValidInput(ref string input)
        {
            try
            {
                if (IsNullOrEmpty(input))
                    //如果是空值,则跳出
                    return true;

                //替换单引号
                input = input.Replace("'", "''").Trim();

                //检测攻击性危险字符串
                var testString =
                    "and |or |exec |insert |select |delete |update |count |chr |mid |master |truncate |char |declare ";
                var testArray = testString.Split('|');
                foreach (var testStr in testArray)
                    if (input.ToLower().IndexOf(testStr) != -1)
                    {
                        //检测到攻击字符串,清空传入的值
                        input = "";
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
    }
}