using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Common.Utility.Extensions.HttpClient;
using Newtonsoft.Json;

namespace Common.Utility.Random.Proxy
{
    /// <summary>
    /// 随机代理
    /// </summary>
    public class RandomProxy
    {
        private HttpClient httpClient;

        public RandomProxy()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                PreAuthenticate = true,
                UseDefaultCredentials = false,
                AutomaticDecompression = DecompressionMethods.GZip,
                UseCookies = false,
                AllowAutoRedirect = true,
                ClientCertificateOptions = ClientCertificateOption.Automatic
            };
            httpClient = new HttpClient(handler);
        }

        public ProxyIp GetRandomIp()
        {
            try
            {
                var json = httpClient.DoGet("https://ip.jiangxianli.com/api/proxy_ip").ReadString();
                if (string.IsNullOrEmpty(json) == false)
                {
                    var josnData = JsonConvert.DeserializeObject<ProxyResponse>(json);
                    if (josnData != null && josnData.Code == 0)
                    {
                        return josnData.Data;
                    }
                }
            }
            catch
            {
                // ignored
            }

            return null;
        }

        public List<ProxyIp> GetRandomIps()
        {
            try
            {
                var json = httpClient.DoGet("https://ip.jiangxianli.com/api/proxy_ips").ReadString();
                if (string.IsNullOrEmpty(json) == false)
                {
                    var josnData = JsonConvert.DeserializeObject<ProxysResponse>(json);
                    if (josnData != null && josnData.Code == 0)
                    {
                        return josnData.Data.Data;
                    }
                }
            }
            catch
            {
                // ignored
            }

            return new List<ProxyIp>();
        }
    }

    public class ProxyResponse
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public ProxyIp Data { get; set; }
    }

    public class ProxysResponse
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public ProxyListIP Data { get; set; }

        public class ProxyListIP
        {
            [JsonProperty("current_page")] public string CurrentPage { get; set; }
            public List<ProxyIp> Data { get; set; }
        }
    }

    /// <summary>
    /// 代理IP
    /// </summary>
    public class ProxyIp
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [JsonProperty("unique_id")]
        public string UniqueId { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        [JsonProperty("ip_address")]
        public string IpAddress { get; set; }

        /// <summary>
        /// 匿名度
        /// </summary>
        public int Anonymity { get; set; }

        /// <summary>
        /// http/https
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// 移动营业厅
        /// </summary>
        public string Isp { get; set; }

        /// <summary>
        /// 速度
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        /// 验证时间
        /// </summary>
        [JsonProperty("validated_at")]
        public string ValidatedAt { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
    }
}