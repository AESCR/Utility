using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Common.Utility.Utils
{
    /// <summary>
    /// 网络请求封装类
    /// </summary>
    public sealed class HttpUtils
    {
        #region Public Methods

        /// <summary>
        /// 字典转url参数 
        /// </summary>
        /// <param name="myParams"> 字典 </param>
        /// <param name="autoEncoding">是否自动编码</param>
        /// <param name="encoding">编码格式</param>
        /// <returns> </returns>
        public static string ParamsToUrl(IDictionary<string, string> myParams,bool autoEncoding=true, Encoding encoding=null)
        {
           
            if (myParams == null)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            int index = 0;
            foreach (string key in myParams.Keys)
            {
                sb.Append(key);
                sb.Append('=');
                sb.Append(autoEncoding ? UrlEncode(myParams[key], encoding) : myParams[key]);

                index++;
                if (index < myParams.Count)
                {
                    sb.Append('&');
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// url参数 拼接
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="myParams"> 字典 </param>
        /// <param name="autoEncoding">是否自动编码</param>
        /// <param name="encoding">编码格式</param>
        /// <returns> </returns>
        public static string ParamsToUrl(string url,IDictionary<string, string> myParams, bool autoEncoding = true, Encoding encoding = null)
        {

            if (myParams == null)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            int index = 0;
            foreach (string key in myParams.Keys)
            {
                sb.Append(key);
                sb.Append('=');
                sb.Append(autoEncoding ? UrlEncode(myParams[key], encoding) : myParams[key]);

                index++;
                if (index < myParams.Count)
                {
                    sb.Append('&');
                }
            }

            return url+"?"+sb.ToString();
        }
        public static string UrlDeCode(string str, Encoding encoding = null)
        {
            if (encoding == null)
            {
                Encoding utf8 = Encoding.UTF8;
                //首先用utf-8进行解码
                string code = HttpUtility.UrlDecode(str.ToUpper(), utf8);
                //将已经解码的字符再次进行编码.
                string encode = HttpUtility.UrlEncode(code, utf8).ToUpper();
                encoding = str == encode ? Encoding.UTF8 : Encoding.GetEncoding("gb2312");
            }
            return HttpUtility.UrlDecode(str, encoding);
        }

        public static string UrlEncode(string str, Encoding encoding = null)
        {
            if (encoding == null)
            {
                Encoding utf8 = Encoding.UTF8;
                //首先用utf-8进行解码
                string code = HttpUtility.UrlEncode(str.ToUpper(), utf8);
                return code;
            }
            return HttpUtility.UrlEncode(str, encoding);
        }

        /// <summary>
        /// url转字典
        /// </summary>
        /// <param name="url"> 请求地址 </param>
        /// <param name="autoEncoding"></param>
        /// <param name="encoding"></param>
        /// <returns> </returns>
        public static IDictionary<string, string> UrlToParams(string url, bool autoEncoding = true, Encoding encoding = null)
        {
            IDictionary<string, string> dic = new Dictionary<string, string>();
            if (url.LastIndexOf('?') >= 0)
            {
                url = url.Substring(url.LastIndexOf('?') + 1);
            }
            if (url.Contains("&"))
            {
                //开始分割&
                string[] splitList = url.Split('&');
                for (int i = 0; i < splitList.Length; i = i + 1)
                {
                    if (splitList[i].Contains("="))
                    {
                        //开始分割k
                        string[] kv = splitList[i].Split('=');
                        dic.Add(kv[0], autoEncoding ? UrlDeCode(kv[1], encoding) : kv[1]);
                    }
                }
            }
            else
            {
                if (url.Contains("="))
                {
                    //开始分割k
                    string[] kv = url.Split('=');
                    dic.Add(kv[0], autoEncoding ? UrlDeCode(kv[1], encoding) : kv[1]);
                }
            }

            return dic;
        }

        #endregion Public Methods
    }
}