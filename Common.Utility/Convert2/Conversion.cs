using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Common.Utility.Convert2
{
    /// <summary>
    /// 处理数据类型转换，数制转换、编码转换相关的类
    /// </summary>
    public sealed class Conversion
    {
        /// <summary>
        /// BinaryFormatter反序列化
        /// </summary>
        /// <param name="str"> 字符串序列 </param>
        public T BinaryDeserialize<T>(string str)
        {
            var intLen = str.Length / 2;
            var bytes = new byte[intLen];
            for (var i = 0; i < intLen; i++)
            {
                var ibyte = Convert.ToInt32(str.Substring(i * 2, 2), 16);
                bytes[i] = (byte)ibyte;
            }

            var formatter = new BinaryFormatter();
            using (var ms = new MemoryStream(bytes))
            {
                return (T)formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// BinaryFormatter序列化
        /// </summary>
        /// <param name="item"> 对象 </param>
        public string BinarySerializer<T>(T item)
        {
            var formatter = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                formatter.Serialize(ms, item);
                ms.Position = 0;
                var bytes = ms.ToArray();
                var sb = new StringBuilder();
                foreach (var bt in bytes) sb.Append(string.Format("{0:X2}", bt));
                return sb.ToString();
            }
        }

        /// <summary>
        /// 二进制反序列化
        /// </summary>
        /// <param name="data"> 数据缓冲区 </param>
        /// <returns> 对象 </returns>
        public object BinaryToObject(byte[] data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using MemoryStream rems = new MemoryStream(data);
            return formatter.Deserialize(rems);
        }

        /// <summary>
        /// 将byte[]转换成int
        /// </summary>
        /// <param name="data"> 需要转换成整数的byte数组 </param>
        public int BytesToInt32(byte[] data)
        {
            //如果传入的字节数组长度小于4,则返回0
            if (data.Length < 4) return 0;

            //定义要返回的整数
            var num = 0;

            //如果传入的字节数组长度大于4,需要进行处理
            if (data.Length >= 4)
            {
                //创建一个临时缓冲区
                var tempBuffer = new byte[4];

                //将传入的字节数组的前4个字节复制到临时缓冲区
                Buffer.BlockCopy(data, 0, tempBuffer, 0, 4);

                //将临时缓冲区的值转换成整数，并赋给num
                num = BitConverter.ToInt32(tempBuffer, 0);
            }

            //返回整数
            return num;
        }

        /// <summary>
        /// 字节转流
        /// </summary>
        /// <param name="buffer"> </param>
        /// <returns> </returns>
        public Stream BytesToStream(byte[] buffer)
        {
            using Stream stream = new MemoryStream(buffer);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        /// <summary>
        /// 使用指定字符集将byte[]转换成string
        /// </summary>
        /// <param name="bytes"> 要转换的字节数组 </param>
        /// <param name="encoding"> 字符编码 </param>
        public string BytesToString(byte[] bytes, Encoding encoding)
        {
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// 实现各进制数间的转换。ConvertBase("15",10,16)表示将十进制数15转换为16进制的数。
        /// </summary>
        /// <param name="value"> 要转换的值,即原值 </param>
        /// <param name="from"> 原值的进制,只能是2,8,10,16四个值。 </param>
        /// <param name="to"> 要转换到的目标进制，只能是2,8,10,16四个值。 </param>
        public string ConvertBase(string value, int from, int to)
        {
            try
            {
                var intValue = Convert.ToInt32(value, from); //先转成10进制
                var result = Convert.ToString(intValue, to); //再转成目标进制
                if (to == 2)
                {
                    var resultLength = result.Length; //获取二进制的长度
                    switch (resultLength)
                    {
                        case 7:
                            result = "0" + result;
                            break;

                        case 6:
                            result = "00" + result;
                            break;

                        case 5:
                            result = "000" + result;
                            break;

                        case 4:
                            result = "0000" + result;
                            break;

                        case 3:
                            result = "00000" + result;
                            break;
                    }
                }

                return result;
            }
            catch
            {
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                return "0";
            }
        }

        /// <summary>
        /// DataTable 转换为List 集合
        /// </summary>
        /// <typeparam name="TResult"> 类型 </typeparam>
        /// <param name="dt"> DataTable </param>
        /// <returns> </returns>
        public List<T> DataTableToList<T>(DataTable dt) where T : new()
        {
            //创建一个属性的列表
            List<PropertyInfo> prlist = new List<PropertyInfo>();
            //获取TResult的类型实例  反射的入口
            Type t = typeof(T);
            //获得TResult 的所有的Public 属性 并找出TResult属性和DataTable的列名称相同的属性(PropertyInfo) 并加入到属性列表
            Array.ForEach<PropertyInfo>(t.GetProperties(), p => { if (dt.Columns.IndexOf(p.Name) != -1) prlist.Add(p); });

            //创建返回的集合

            List<T> oblist = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                //创建TResult的实例
                T ob = new T();
                //找到对应的数据  并赋值
                prlist.ForEach(p => { if (row[p.Name] != DBNull.Value) p.SetValue(ob, row[p.Name], null); });
                //放入到返回的集合中.
                oblist.Add(ob);
            }
            return oblist;
        }

        /// <summary>
        /// Json转对象
        /// </summary>
        /// <param name="val"> </param>
        /// <typeparam name="T"> </typeparam>
        /// <returns> </returns>
        public T DeserializeObject<T>(string val)
        {
            return JsonConvert.DeserializeObject<T>(val);
        }

        /// <summary>
        /// 文件化XML反序列化
        /// </summary>
        /// <param name="path"> 文件路径 </param>
        public T FileXmlDeserialize<T>(string path)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                fs?.Close();
            }
        }

        /// <summary>
        /// 文件化XML序列化 到文件
        /// </summary>
        /// <param name="obj"> 对象 </param>
        /// <param name="filename"> 文件路径 </param>
        public void FileXmlSerializer(object obj, string filename)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                var serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                fs?.Close();
            }
        }

        /// <summary>
        /// HTML转行成TEXT
        /// </summary>
        /// <param name="strHtml"> </param>
        /// <returns> </returns>
        public string HtmlToTxt(string strHtml)
        {
            string[] aryReg =
            {
                @"<script[^>]*?>.*?</script>",
                @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
                @"([\r\n])[\s]+",
                @"&(quot|#34);",
                @"&(amp|#38);",
                @"&(lt|#60);",
                @"&(gt|#62);",
                @"&(nbsp|#160);",
                @"&(iexcl|#161);",
                @"&(cent|#162);",
                @"&(pound|#163);",
                @"&(copy|#169);",
                @"&#(\d+);",
                @"-->",
                @"<!--.*\n"
            };

            var newReg = aryReg[0];
            var strOutput = strHtml;
            for (var i = 0; i < aryReg.Length; i++)
            {
                var regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, string.Empty);
            }
            strOutput.Replace("<", "");
            strOutput.Replace(">", "");
            strOutput.Replace("\r\n", "");
            return strOutput;
        }

        /// <summary>
        /// 将数组转换为字符串
        /// </summary>
        /// <param name="list"> List </param>
        /// <param name="speater"> 分隔符 </param>
        /// <returns> String </returns>
        public string ListToStr(List<string> list, string speater)
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

        ///<summary>
        /// 二进制序列化
        /// </summary>
        /// <param name="data">要序列化的对象</param>
        /// <returns>返回存放序列化后的数据缓冲区</returns>
        public byte[] ObjectToBinary(object o)
        {
            using MemoryStream rems = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(rems, o);
            return rems.GetBuffer();
        }

        /// <summary>
        /// 将对象属性转换为key-value对
        /// </summary>
        /// <param name="o"> </param>
        /// <returns> </returns>
        public Dictionary<String, Object> ObjectToDictionary(object o)
        {
            Dictionary<String, Object> map = new Dictionary<string, object>();
            Type t = o.GetType();
            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in pi)
            {
                MethodInfo mi = p.GetGetMethod();
                if (mi != null && mi.IsPublic)
                {
                    map.Add(p.Name, mi.Invoke(o, new Object[] { }));
                }
            }
            return map;
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="expression"> 要转换的字符串 </param>
        /// <param name="defValue"> 缺省值 </param>
        /// <returns> 转换后的int类型结果 </returns>
        public int ObjToInt(object expression, int defValue)
        {
            if (expression != null)
                return StrToInt(expression.ToString(), defValue);

            return defValue;
        }

        /// <summary>
        /// 指定字符串的固定长度，如果字符串小于固定长度， 则在字符串的前面补足零，可设置的固定长度最大为9位
        /// </summary>
        /// <param name="text"> 原始字符串 </param>
        /// <param name="limitedLength"> 字符串的固定长度 </param>
        public string RepairZero(string text, int limitedLength)
        {
            //补足0的字符串
            var temp = "";

            //补足0
            for (var i = 0; i < limitedLength - text.Length; i++) temp += "0";

            //连接text
            temp += text;

            //返回补足0的字符串
            return temp;
        }

        /// <summary>
        /// 对象转Json
        /// </summary>
        /// <param name="o"> </param>
        /// <returns> </returns>
        public string SerializeObject(object o)
        {
            return JsonConvert.SerializeObject(o);
        }

        /// <summary>
        /// 使用指定字符集将string转换成byte[]
        /// </summary>
        /// <param name="text"> 要转换的字符串 </param>
        /// <param name="encoding"> 字符编码 </param>
        public byte[] StringToBytes(string text, Encoding encoding)
        {
            return encoding.GetBytes(text);
        }

        /// <summary>
        /// 将字符串转换为数组
        /// </summary>
        /// <param name="str"> 字符串 </param>
        /// <returns> 字符串数组 </returns>
        public string[] StrToArray(string str)
        {
            return str.Split(new char[',']);
        }

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue"> 要转换的字符串 </param>
        /// <param name="defValue"> 缺省值 </param>
        /// <returns> 转换后的bool类型结果 </returns>
        public bool StrToBool(string expression, bool defValue)
        {
            if (expression != null)
            {
                if (string.Compare(expression, "true", true) == 0)
                    return true;
                if (string.Compare(expression, "false", true) == 0)
                    return false;
            }

            return defValue;
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="str"> 要转换的字符串 </param>
        /// <param name="defValue"> 缺省值 </param>
        /// <returns> 转换后的int类型结果 </returns>
        public DateTime StrToDateTime(string str, DateTime defValue)
        {
            if (!string.IsNullOrEmpty(str))
            {
                DateTime dateTime;
                if (DateTime.TryParse(str, out dateTime))
                    return dateTime;
            }

            return defValue;
        }

        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="str"> 要转换的字符串 </param>
        /// <returns> 转换后的int类型结果 </returns>
        public DateTime StrToDateTime(string str)
        {
            return StrToDateTime(str, DateTime.Now);
        }

        /// <summary>
        /// string型转换为decimal型
        /// </summary>
        /// <param name="strValue"> 要转换的字符串 </param>
        /// <param name="defValue"> 缺省值 </param>
        /// <returns> 转换后的decimal类型结果 </returns>
        public decimal StrToDecimal(string expression, decimal defValue)
        {
            if (expression == null || expression.Length > 10)
                return defValue;

            var intValue = defValue;
            if (expression != null)
            {
                var IsDecimal = Regex.IsMatch(expression, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsDecimal)
                    decimal.TryParse(expression, out intValue);
            }

            return intValue;
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue"> 要转换的字符串 </param>
        /// <param name="defValue"> 缺省值 </param>
        /// <returns> 转换后的int类型结果 </returns>
        public float StrToFloat(string expression, float defValue)
        {
            if (expression == null || expression.Length > 10)
                return defValue;

            var intValue = defValue;
            if (expression != null)
            {
                var IsFloat = Regex.IsMatch(expression, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                    float.TryParse(expression, out intValue);
            }

            return intValue;
        }

        /// <summary>
        /// 将字符串转换为Int32类型
        /// </summary>
        /// <param name="expression"> 要转换的字符串 </param>
        /// <param name="defValue"> 缺省值 </param>
        /// <returns> 转换后的int类型结果 </returns>
        public int StrToInt(string expression, int defValue)
        {
            if (string.IsNullOrEmpty(expression) || expression.Trim().Length >= 11 ||
                !Regex.IsMatch(expression.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defValue;

            int rv;
            if (int.TryParse(expression, out rv))
                return rv;

            return Convert.ToInt32(StrToFloat(expression, defValue));
        }

        /// <summary>
        /// 文本化XML反序列化
        /// </summary>
        /// <param name="str"> 字符串序列 </param>
        public T XmlDeserialize<T>(string str)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (XmlReader reader = new XmlTextReader(new StringReader(str)))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// 文本化XML序列化
        /// </summary>
        /// <param name="item"> 对象 </param>
        public string XmlSerializer<T>(T item)
        {
            var serializer = new XmlSerializer(item.GetType());
            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb))
            {
                serializer.Serialize(writer, item);
                return sb.ToString();
            }
        }
    }
}