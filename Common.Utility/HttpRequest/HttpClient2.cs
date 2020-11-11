using Common.Utility.Extensions.HttpClient;
using Common.Utility.Extensions.System;
using Common.Utility.Random.Proxy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Common.Utility.HttpRequest
{
    /// <summary>
    /// 请求帮助类 http://httpbin.org/ docker run -p 80:80 kennethreitz/httpbin
    /// </summary>
    public class HttpClient2
    {
        /// <summary>
        /// 不同url分配不同HttpClient
        /// </summary>
        private readonly Dictionary<string, HttpClient> _dic = new Dictionary<string, HttpClient>();

        private readonly RandomProxy _proxy = new RandomProxy();
        private readonly Action<HttpClientHandler> handAction;
        private bool _randomProxy = false;
        public static readonly HttpClient Instance;

        static HttpClient2()
        {
            Instance = new System.Net.Http.HttpClient();
        }

        public HttpClient2(Action<HttpClientHandler> option = null, bool randomProxy = false)
        {
            handAction = option;
            _randomProxy = randomProxy;
        }

        private HttpClient GetClient(string url)
        {
            var uri = new Uri(url);
            var key = uri.Scheme + uri.Host;
            if (!_dic.Keys.Contains(key))
            {
                _dic.Add(key, new HttpClient());
            }
            return _dic[key];
        }

        private HttpClient GetHttpClient(string url)
        {
            if (_randomProxy)
            {
                return GetProxyClient(url);
            }
            else
            {
                return GetClient(url);
            }
        }

        private HttpClient GetProxyClient(string url)
        {
            var uri = new Uri(url);
            var key = uri.Scheme + uri.Host;
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefault();
            string webProxy = _proxy.GetRandomIp().WebProxyUrl;
            handler.UseWebProxy(webProxy);
            handAction?.Invoke(handler);
            var tempClient = new HttpClient(handler);
            tempClient.Timeout = TimeSpan.FromMilliseconds(900);
            return tempClient;
        }

        /// <summary>
        /// Delete请求
        /// </summary>
        /// <param name="url"> url地址 </param>
        /// <param name="headers"> </param>
        /// <returns> </returns>
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

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url"> url地址 </param>
        /// <param name="headers"> webapi做用户认证 </param>
        /// <returns> </returns>
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
        /// Post请求
        /// </summary>
        /// <param name="url"> url地址 </param>
        /// <param name="jsonString"> 请求参数（Json字符串） </param>
        /// <param name="headers"> webapi做用户认证 </param>
        /// <returns> </returns>
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
        /// <typeparam name="T"> </typeparam>
        /// <param name="url"> url地址 </param>
        /// <param name="content"> 请求参数 </param>
        /// <param name="headers"> webapi做用户认证 </param>
        /// <returns> </returns>
        public async Task<HttpResponseMessage> PostAsync<T>(string url, T content, Dictionary<string, string> headers = null) where T : class
        {
            return await PostAsync(url, JsonConvert.SerializeObject(content), headers);
        }

        /// <summary>
        /// Put请求
        /// </summary>
        /// <param name="url"> url地址 </param>
        /// <param name="jsonString"> 请求参数（Json字符串） </param>
        /// <param name="headers"> webapi做用户认证 </param>
        /// <returns> </returns>
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
        /// <typeparam name="T"> </typeparam>
        /// <param name="url"> url地址 </param>
        /// <param name="content"> 请求参数 </param>
        /// <param name="headers"> webapi做用户认证 </param>
        /// <returns> </returns>
        public async Task<HttpResponseMessage> PutAsync<T>(string url, T content, Dictionary<string, string> headers = null)
        {
            return await PutAsync(url, JsonConvert.SerializeObject(content), headers);
        }
    }
}