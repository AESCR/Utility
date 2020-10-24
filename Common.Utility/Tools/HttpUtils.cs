using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Utility.Tools
{
    /// <summary>
    /// 网络请求封装类
    /// </summary>
    public sealed class HttpUtils
    {
        /// <summary>
        /// 字典转url参数 使用前先编码
        /// </summary>
        /// <param name="myParams">字典</param>
        /// <returns></returns>
        public static string ParamsToUrl(IDictionary<string, string> myParams)
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
                sb.Append(myParams[key]);
                index++;
                if (index<myParams.Count)
                {
                    sb.Append('&');
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// url转字典
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public static IDictionary<string, string> UrlToParams(string url)
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
                for (int i = 0; i < splitList.Length; i = i +1)
                {
                    if (splitList[i].Contains("="))
                    {
                        //开始分割k
                        string[] kv = splitList[i].Split('=');
                        dic.Add(kv[0], kv[1]);
                    }
                }
            }
            else
            {
                if (url.Contains("="))
                {
                    //开始分割k
                    string[] kv = url.Split('=');
                    dic.Add(kv[0], kv[1]);
                }
            }

            return dic;
        }
    }

    
}