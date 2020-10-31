using Common.Utility.Extensions.System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Common.Utility.Extensions.HttpClient
{
    public static class HttpClientEx
    {
        #region Public Methods

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="httpClient"> </param>
        /// <param name="url"> 请求url </param>
        /// <returns> 返回HttpResponseMessage 否则null </returns>
        public static Task<HttpResponseMessage> DoGet(this global::System.Net.Http.HttpClient httpClient, string url)
        {
            var taskResponse = httpClient.GetAsync(url);
            return taskResponse;
        }

        /// <summary>
        /// Post请求 内容类型:application/json
        /// </summary>
        /// <param name="httpClient"> </param>
        /// <param name="url"> 请求url </param>
        /// <param name="postData"> json内容 </param>
        /// <param name="contentHeader"> body header </param>
        /// <returns> 返回HttpResponseMessage </returns>
        public static Task<HttpResponseMessage> DoPost<T>(this global::System.Net.Http.HttpClient httpClient, string url, T postData,
            Dictionary<string, string> contentHeader = null) where T : class
        {
            var jsonData = JsonConvert.SerializeObject(postData);
            HttpContent httpContent = new StringContent(jsonData);
            if (contentHeader != null)
                foreach (var headerKey in contentHeader.Keys)
                    httpContent.Headers.Add(headerKey, contentHeader[headerKey]);
            var taskResponse = httpClient.PostAsync(url, httpContent);
            return taskResponse;
        }

        /// <summary>
        /// Post请求 内容类型:application/json
        /// </summary>
        /// <param name="httpClient"> </param>
        /// <param name="url"> 请求url </param>
        /// <param name="jsonData"> json内容 </param>
        /// <param name="contentHeader"> body header </param>
        /// <returns> 返回HttpResponseMessage </returns>
        public static Task<HttpResponseMessage> DoPost(this global::System.Net.Http.HttpClient httpClient, string url, string jsonData,
            Dictionary<string, string> contentHeader = null)
        {
            HttpContent httpContent = new StringContent(jsonData);
            if (contentHeader != null)
                foreach (var headerKey in contentHeader.Keys)
                    httpContent.Headers.Add(headerKey, contentHeader[headerKey]);
            var taskResponse = httpClient.PostAsync(url, httpContent);
            return taskResponse;
        }

        /// <summary>
        /// Post请求 内容类型:application/x-www-form-urlencoded
        /// </summary>
        /// <param name="httpClient"> </param>
        /// <param name="url"> 请求url </param>
        /// <param name="formData"> 数据参数 </param>
        /// <param name="contentHeader"> </param>
        /// <returns> 返回HttpResponseMessage 否则null </returns>
        /// <returns> </returns>
        public static Task<HttpResponseMessage> DoPost(this global::System.Net.Http.HttpClient httpClient, string url,
           Dictionary<string, string> formData, Dictionary<string, string> contentHeader = null)
        {
            IEnumerable<KeyValuePair<string, string>> data = formData.ToKeyValuePairCollection();
            HttpContent httpContent = new FormUrlEncodedContent(data);
            if (contentHeader != null)
                foreach (var headerKey in contentHeader.Keys)
                    httpContent.Headers.Add(headerKey, contentHeader[headerKey]);
            var taskResponse = httpClient.PostAsync(url, httpContent);
            return taskResponse;
        }

        /// <summary>
        /// Set UserAgent
        /// </summary>
        /// <param name="this"> </param>
        /// <param name="userAgent"> </param>
        public static void UseUserAgent(this global::System.Net.Http.HttpClient @this, string userAgent = null)
        {
            @this.DefaultRequestHeaders.UserAgent.Clear();
            @this.DefaultRequestHeaders.UserAgent.TryParseAdd(string.IsNullOrEmpty(userAgent)
                ? "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.61 Safari/537.36"
                : userAgent);
        }

        #endregion Public Methods
    }
}