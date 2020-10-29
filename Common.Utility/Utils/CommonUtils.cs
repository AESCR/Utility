using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Utility.Tools
{
    /// <summary>
    ///     共用工具类
    /// </summary>
    public static class CommonUtils
    {
        #region Public Methods

        /// <summary>
        ///     截取指定长度字符串
        /// </summary>
        /// <param name="inputString">要处理的字符串</param>
        /// <param name="len">指定长度</param>
        /// <returns>返回处理后的字符串</returns>
        public static string ClipString(string inputString, int len)
        {
            var isShowFix = false;
            if (len % 2 == 1)
            {
                isShowFix = true;
                len--;
            }

            var ascii = new ASCIIEncoding();
            var tempLen = 0;
            var tempString = "";
            var s = ascii.GetBytes(inputString);
            for (var i = 0; i < s.Length; i++)
            {
                if (s[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }

            var mybyte = Encoding.Default.GetBytes(inputString);
            if (isShowFix && mybyte.Length > len)
                tempString += "…";
            return tempString;
        }

        /// <summary>
        ///     获得两个日期的间隔
        /// </summary>
        /// <param name="DateTime1">日期一。</param>
        /// <param name="DateTime2">日期二。</param>
        /// <returns>日期间隔TimeSpan。</returns>
        public static TimeSpan DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            var ts1 = new TimeSpan(DateTime1.Ticks);
            var ts2 = new TimeSpan(DateTime2.Ticks);
            var ts = ts1.Subtract(ts2).Duration();
            return ts;
        }
        /// <summary>
        ///     格式化日期时间
        /// </summary>
        /// <param name="dateTime1">日期时间</param>
        /// <param name="dateMode">显示模式</param>
        /// <returns>0-9种模式的日期</returns>
        public static string FormatDate(DateTime dateTime1, string dateMode)
        {
            switch (dateMode)
            {
                case "0":
                    return dateTime1.ToString("yyyy-MM-dd");

                case "1":
                    return dateTime1.ToString("yyyy-MM-dd HH:mm:ss");

                case "2":
                    return dateTime1.ToString("yyyy/MM/dd");

                case "3":
                    return dateTime1.ToString("yyyy年MM月dd日");

                case "4":
                    return dateTime1.ToString("MM-dd");

                case "5":
                    return dateTime1.ToString("MM/dd");

                case "6":
                    return dateTime1.ToString("MM月dd日");

                case "7":
                    return dateTime1.ToString("yyyy-MM");

                case "8":
                    return dateTime1.ToString("yyyy/MM");

                case "9":
                    return dateTime1.ToString("yyyy年MM月");

                default:
                    return dateTime1.ToString();
            }
        }

        /// <summary>
        ///     删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(string str, string strchar)
        {
            return str.Substring(0, str.LastIndexOf(strchar));
        }

        /// <summary>
        ///     删除最后结尾的一个逗号
        /// </summary>
        public static string DelLastComma(string str)
        {
            return str.Substring(0, str.LastIndexOf(","));
        }

        /// <summary>
        ///     把 List<string> 按照分隔符组装成 string
        /// </summary>
        /// <param name="list"></param>
        /// <param name="speater"></param>
        /// <returns></returns>
        public static string GetArrayStr(List<string> list, string speater)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < list.Count; i++)
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }

            return sb.ToString();
        }

        /// <summary>
        ///     得到数组列表以逗号分隔的字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetArrayStr(List<int> list)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < list.Count; i++)
                if (i == list.Count - 1)
                {
                    sb.Append(list[i].ToString());
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(",");
                }

            return sb.ToString();
        }

        /// <summary>
        ///     得到数组列表以逗号分隔的字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetArrayValueStr(Dictionary<int, int> list)
        {
            var sb = new StringBuilder();
            foreach (var kvp in list) sb.Append(kvp.Value + ",");
            if (list.Count > 0)
                return DelLastComma(sb.ToString());
            return "";
        }

        /// <summary>
        ///     将字符串样式转换为纯字符串
        /// </summary>
        /// <param name="StrList"></param>
        /// <param name="SplitString"></param>
        /// <returns></returns>
        public static string GetCleanStyle(string StrList, string SplitString)
        {
            var RetrunValue = "";
            //如果为空，返回空值
            if (StrList == null)
            {
                RetrunValue = "";
            }
            else
            {
                //返回去掉分隔符
                var NewString = "";
                NewString = StrList.Replace(SplitString, "");
                RetrunValue = NewString;
            }

            return RetrunValue;
        }

        /// <summary>
        ///     将字符串转换为新样式
        /// </summary>
        /// <param name="StrList"></param>
        /// <param name="NewStyle"></param>
        /// <param name="SplitString"></param>
        /// <param name="Error"></param>
        /// <returns></returns>
        public static string GetNewStyle(string StrList, string NewStyle, string SplitString, out string Error)
        {
            var ReturnValue = "";
            //如果输入空值，返回空，并给出错误提示
            if (StrList == null)
            {
                ReturnValue = "";
                Error = "请输入需要划分格式的字符串";
            }
            else
            {
                //检查传入的字符串长度和样式是否匹配,如果不匹配，则说明使用错误。给出错误信息并返回空值
                var strListLength = StrList.Length;
                var NewStyleLength = GetCleanStyle(NewStyle, SplitString).Length;
                if (strListLength != NewStyleLength)
                {
                    ReturnValue = "";
                    Error = "样式格式的长度与输入的字符长度不符，请重新输入";
                }
                else
                {
                    //检查新样式中分隔符的位置
                    var Lengstr = "";
                    for (var i = 0; i < NewStyle.Length; i++)
                        if (NewStyle.Substring(i, 1) == SplitString)
                            Lengstr = Lengstr + "," + i;
                    if (Lengstr != "") Lengstr = Lengstr.Substring(1);
                    //将分隔符放在新样式中的位置
                    var str = Lengstr.Split(',');
                    foreach (var bb in str) StrList = StrList.Insert(int.Parse(bb), SplitString);
                    //给出最后的结果
                    ReturnValue = StrList;
                    //因为是正常的输出，没有错误
                    Error = "";
                }
            }

            return ReturnValue;
        }

        /// <summary>
        ///     把字符串按照分隔符转换成 List
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="speater">分隔符</param>
        /// <param name="toLower">是否转换为小写</param>
        /// <returns></returns>
        public static List<string> GetStrArray(string str, char speater, bool toLower)
        {
            var list = new List<string>();
            var ss = str.Split(speater);
            foreach (var s in ss)
                if (!string.IsNullOrEmpty(s) && s != speater.ToString())
                {
                    var strVal = s;
                    if (toLower) strVal = s.ToLower();
                    list.Add(strVal);
                }

            return list;
        }

        /// <summary>
        ///     把字符串转 按照, 分割 换为数据
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] GetStrArray(string str)
        {
            return str.Split(new[] { ',' });
        }

        /// <summary>
        ///     把字符串按照指定分隔符装成 List 去除重复
        /// </summary>
        /// <param name="o_str"></param>
        /// <param name="sepeater"></param>
        /// <returns></returns>
        public static List<string> GetSubStringList(string o_str, char sepeater)
        {
            var list = new List<string>();
            var ss = o_str.Split(sepeater);
            foreach (var s in ss)
                if (!string.IsNullOrEmpty(s) && s != sepeater.ToString())
                    list.Add(s);
            return list;
        }

     

        /// <summary>
        ///     判断对象是否为空，为空返回true
        /// </summary>
        /// <typeparam name="T">要验证的对象的类型</typeparam>
        /// <param name="data">要验证的对象</param>
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
        ///     判断对象是否为空，为空返回true
        /// </summary>
        /// <param name="data">要验证的对象</param>
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
        ///     分割字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="splitstr"></param>
        /// <returns></returns>
        public static string[] SplitMulti(string str, string splitstr)
        {
            string[] strArray = null;
            if (str != null && str != "") strArray = new Regex(splitstr).Split(str);
            return strArray;
        }

        public static string SqlSafeString(string String, bool IsDel)
        {
            if (IsDel)
            {
                String = String.Replace("'", "");
                String = String.Replace("\"", "");
                return String;
            }

            String = String.Replace("'", "&#39;");
            String = String.Replace("\"", "&#34;");
            return String;
        }

        /// <summary>
        ///     得到字符串长度，一个汉字长度为2
        /// </summary>
        /// <param name="inputString">参数字符串</param>
        /// <returns></returns>
        public static int StrLength(string inputString)
        {
            var ascii = new ASCIIEncoding();
            var tempLen = 0;
            var s = ascii.GetBytes(inputString);
            for (var i = 0; i < s.Length; i++)
                if (s[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;
            return tempLen;
        }

        /// <summary>
        ///     转半角的函数(SBC case)
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static string ToDBC(string input)
        {
            var c = input.ToCharArray();
            for (var i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }

                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }

            return new string(c);
        }

        /// <summary>
        ///     转全角的函数(SBC case)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(string input)
        {
            //半角转全角：
            var c = input.ToCharArray();
            for (var i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }

                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }

            return new string(c);
        }

        #endregion Public Methods
           #region Public Methods
           /// <summary>
           /// 将object对象转换为实体对象
           /// </summary>
           /// <typeparam name="T">实体对象类名</typeparam>
           /// <param name="asObject">object对象</param>
           /// <returns></returns>
           public static T ConvertObject<T>(object asObject) where T : new()
           {
               //创建实体对象实例
               var t = Activator.CreateInstance<T>();
               if (asObject != null)
               {
                   Type type = asObject.GetType();
                   //遍历实体对象属性
                   foreach (var info in typeof(T).GetProperties())
                   {
                       object obj = null;
                       //取得object对象中此属性的值
                       var val = type.GetProperty(info.Name)?.GetValue(asObject);
                       if (val != null)
                       {
                           //非泛型
                           if (!info.PropertyType.IsGenericType)
                               obj = Convert.ChangeType(val, info.PropertyType);
                           else//泛型Nullable<>
                           {
                               Type genericTypeDefinition = info.PropertyType.GetGenericTypeDefinition();
                               if (genericTypeDefinition == typeof(Nullable<>))
                               {
                                   obj = Convert.ChangeType(val, Nullable.GetUnderlyingType(info.PropertyType));
                               }
                               else
                               {
                                   obj = Convert.ChangeType(val, info.PropertyType);
                               }
                           }
                           info.SetValue(t, obj, null);
                       }
                   }
               }
               return t;
           }
        /// <summary>
        ///     转中文大写数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConvertNumToZhUpperCase(decimal value)
        {
            string[] numList = { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
            string[] unitList = { "分", "角", "元", "拾", "佰", "仟", "万", "拾", "佰", "仟", "亿", "拾", "佰", "仟" };

            var money = value;
            if (money == 0) return "零元整";

            var strMoney = new StringBuilder();
            //只取小数后2位

            var strNum = decimal.Truncate(money * 100).ToString();

            var len = strNum.Length;
            var zero = 0;
            for (var i = 0; i < len; i++)
            {
                var num = int.Parse(strNum.Substring(i, 1));
                var unitNum = len - i - 1;

                if (num == 0)
                {
                    zero++;
                    if (unitNum == 2 || unitNum == 6 || unitNum == 10)
                    {
                        if (unitNum == 2 || zero < 4)
                            strMoney.Append(unitList[unitNum]);
                        zero = 0;
                    }
                }
                else
                {
                    if (zero > 0)
                    {
                        strMoney.Append(numList[0]);
                        zero = 0;
                    }

                    strMoney.Append(numList[num]);
                    strMoney.Append(unitList[unitNum]);
                }
            }

            if (zero > 0)
                strMoney.Append("整");

            return strMoney.ToString();
        }

        public static decimal? ParseToDecimalValue(object decimalObj)
        {
            if (decimalObj == null) return null;
            decimal decValue;
            if (!decimal.TryParse(decimalObj.ToString(), out decValue)) return null;
            return decValue;
        }

        /// <summary>
        ///     截取指定位数
        /// </summary>
        /// <param name="d"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static decimal ToFixed(decimal d, int s)
        {
            var sp = Convert.ToDecimal(Math.Pow(10, s));
            return Math.Truncate(d) + Math.Floor((d - Math.Truncate(d)) * sp) / sp;
        }

        /// <summary>
        ///     截取指定位数
        /// </summary>
        /// <param name="d"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static double ToFixed(double d, int s)
        {
            var sp = Math.Pow(10, s);
            return Math.Truncate(d) + Math.Floor((d - Math.Truncate(d)) * sp) / sp;
        }
     
        #endregion Public Methods
    }
}