using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Common.Utility.HttpRequest
{
    /// <summary>
    /// 请求帮助类
    /// </summary>
    public class HttpClient2
    {
        public static readonly System.Net.Http.HttpClient Instance;
        static HttpClient2()
        {
            /*HttpClientHandler handler = new HttpClientHandler()
            {
                Proxy = new WebProxy("http://36.35.95.107:8118",false),
                PreAuthenticate = true,
                UseDefaultCredentials = false,
                AutomaticDecompression = DecompressionMethods.GZip,
                UseCookies = false,
                AllowAutoRedirect = true,
                ClientCertificateOptions = ClientCertificateOption.Automatic
            };*/
            Instance = new System.Net.Http.HttpClient();
        }
        /// <summary>
        /// 不同url分配不同HttpClient
        /// </summary>
        public static Dictionary<string, System.Net.Http.HttpClient> dictionary = new Dictionary<string, System.Net.Http.HttpClient>();

        private System.Net.Http.HttpClient GetHttpClient(string url)
        {
            var uri = new Uri(url);
            var key = uri.Scheme + uri.Host;
            if (!dictionary.Keys.Contains(key))
                dictionary.Add(key, new System.Net.Http.HttpClient());
            return dictionary[key];
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="jsonString">请求参数（Json字符串）</param>
        /// <param name="headers">webapi做用户认证</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostAsync(string url, string jsonString, Dictionary<string, string> headers = null)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
                jsonString = "{}";
            StringContent content = new StringContent(jsonString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            if (headers != null && headers.Any())
            {
                //如果有headers认证等信息，则每个请求实例一个HttpClient
                using (System.Net.Http.HttpClient http = new System.Net.Http.HttpClient())
                {
                    foreach (var item in headers)
                    {
                        http.DefaultRequestHeaders.Remove(item.Key);
                        http.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                    }
                    return await http.PostAsync(new Uri(url), content);
                }
            }
            else
            {
                return await GetHttpClient(url).PostAsync(new Uri(url), content);
            }
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">url地址</param>
        /// <param name="content">请求参数</param>
        /// <param name="headers">webapi做用户认证</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostAsync<T>(string url, T content, Dictionary<string, string> headers = null) where T : class
        {
            return await PostAsync(url, JsonConvert.SerializeObject(content), headers);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="headers">webapi做用户认证</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetAsync(string url, Dictionary<string, string> headers = null)
        {
            if (headers != null && headers.Any())
            {
                //如果有headers认证等信息，则每个请求实例一个HttpClient
                using (System.Net.Http.HttpClient http = new System.Net.Http.HttpClient())
                {
                    foreach (var item in headers)
                    {
                        http.DefaultRequestHeaders.Remove(item.Key);
                        http.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                    }
                    return await http.GetAsync(url);
                }
            }
            else
            {
                return await GetHttpClient(url).GetAsync(url);
            }
        }

        /// <summary>
        /// Put请求
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="jsonString">请求参数（Json字符串）</param>
        /// <param name="headers">webapi做用户认证</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PutAsync(string url, string jsonString, Dictionary<string, string> headers = null)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
                jsonString = "{}";
            StringContent content = new StringContent(jsonString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (headers != null && headers.Any())
            {
                //如果有headers认证等信息，则每个请求实例一个HttpClient
                using (System.Net.Http.HttpClient http = new System.Net.Http.HttpClient())
                {
                    foreach (var item in headers)
                    {
                        http.DefaultRequestHeaders.Remove(item.Key);
                        http.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                    }
                    return await http.PutAsync(url, content);
                }
            }
            else
            {
                return await GetHttpClient(url).PutAsync(url, content);
            }
        }

        /// <summary>
        /// Put请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">url地址</param>
        /// <param name="content">请求参数</param>
        /// <param name="headers">webapi做用户认证</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PutAsync<T>(string url, T content, Dictionary<string, string> headers = null)
        {
            return await PutAsync(url, JsonConvert.SerializeObject(content), headers);
        }

        /// <summary>
        /// Delete请求
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> DeleteAsync(string url, Dictionary<string, string> headers = null)
        {
            if (headers != null && headers.Any())
            {
                //如果有headers认证等信息，则每个请求实例一个HttpClient
                using (System.Net.Http.HttpClient http = new System.Net.Http.HttpClient())
                {
                    foreach (var item in headers)
                    {
                        http.DefaultRequestHeaders.Remove(item.Key);
                        http.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                    }
                    return await http.DeleteAsync(url);
                }
            }
            else
            {
                return await GetHttpClient(url).DeleteAsync(url);
            }
        }
    }
}
