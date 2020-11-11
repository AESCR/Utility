using Common.Utility.Autofac;
using Common.Utility.Extensions.HttpClient;
using Common.Utility.Memory;
using Common.Utility.Memory.Cache;
using Common.Utility.Memory.Model;
using Common.Utility.Random.Num;
using Common.Utility.SystemExtensions;
using Common.Utility.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;

namespace Common.Utility.Random.Proxy
{
    public enum CountryEnum
    {
        [Description("中国")]
        China,

        [Description("日本")]
        Japanese,

        [Description("美国")]
        America,

        All,
    }

    /// <summary>
    /// 代理IP
    /// </summary>
    public class ProxyIp
    {
        /// <summary>
        /// 匿名度
        /// </summary>
        public int Anonymity { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        [JsonProperty("ip_address")]
        public string IpAddress { get; set; }

        /// <summary>
        /// 移动营业厅
        /// </summary>
        public string Isp { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// http/https
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// 速度
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        /// 唯一标识
        /// </summary>
        [JsonProperty("unique_id")]
        public string UniqueId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        /// <summary>
        /// 验证时间
        /// </summary>
        [JsonProperty("validated_at")]
        public string ValidatedAt { get; set; }

        public string WebProxyUrl => $"{Protocol}://{Ip}:{Port}";
    }

    public class ProxyResponse
    {
        public int Code { get; set; }
        public ProxyIp Data { get; set; }
        public string Msg { get; set; }
    }

    public class ProxysResponse
    {
        public int Code { get; set; }

        public ProxyListIP Data { get; set; }

        public string Msg { get; set; }

        public class ProxyListIP
        {
            [JsonProperty("current_page")] public string CurrentPage { get; set; }

            public List<ProxyIp> Data { get; set; }
        }
    }

    /// <summary>
    /// 随机代理
    /// </summary>
    public class RandomProxy : ISingletonDependency
    {
        private readonly HttpClient httpClient;
        private readonly IMemoryCache memory;
        private readonly int minutes;
        private readonly RandomNum random;

        public RandomProxy(int minutes = 1)
        {
            this.minutes = minutes;
            var handler = new HttpClientHandler
            {
                PreAuthenticate = true,
                UseDefaultCredentials = false,
                AutomaticDecompression = DecompressionMethods.GZip,
                UseCookies = false,
                AllowAutoRedirect = true,
                ClientCertificateOptions = ClientCertificateOption.Automatic
            };
            httpClient = new HttpClient(handler);
            memory = new MemoryCache2();
            random = new RandomNum();
        }

        private List<ProxyIp> GetRandomIps(CountryEnum country = CountryEnum.China)
        {
            var url = "https://ip.jiangxianli.com/api/proxy_ips";
            var param = new Dictionary<string, string>();
            if (country != CountryEnum.All)
            {
                param.Add("country", country.ToDescription());
                url = url + "?" + HttpUtils.ParamsToUrl(param);
            }
            var json = httpClient.DoGet(url).ReadString(out _);
            if (string.IsNullOrEmpty(json) == false)
            {
                var josnData = JsonConvert.DeserializeObject<ProxysResponse>(json);
                if (josnData != null && josnData.Code == 0)
                {
                    memory.Add(MemoryEnum.Proxy.GetMemoryKey(), josnData.Data.Data, TimeSpan.FromMinutes(minutes));
                    if (country != CountryEnum.All)
                    {
                        memory.Add(MemoryEnum.Proxy.GetMemoryKey(country.GetEnumName()), josnData.Data.Data, TimeSpan.FromMinutes(minutes));
                    }
                    return josnData.Data.Data;
                }
            }
            throw new Exception("https://ip.jiangxianli.com not allow");
        }

        public ProxyIp GetRandomIp(bool cache = true)
        {
            if (memory.Exists(MemoryEnum.Proxy.GetMemoryKey()))
            {
                var proxys = memory.Get<List<ProxyIp>>(MemoryEnum.Proxy.GetMemoryKey());
                if (proxys.Count > 0)
                {
                    var rint = random.GetRandomInt(0, proxys.Count);
                    return proxys[rint];
                }
            }
            if (cache)
            {
                var proxys = GetRandomIps();
                if (proxys.Count > 0)
                {
                    var rint = random.GetRandomInt(0, proxys.Count);
                    return proxys[rint];
                }
            }
            else
            {
                var url = "https://ip.jiangxianli.com/api/proxy_ip";
                var json = httpClient.DoGet(url).ReadString(out _);
                if (string.IsNullOrEmpty(json) == false)
                {
                    var josnData = JsonConvert.DeserializeObject<ProxyResponse>(json);
                    if (josnData != null && josnData.Code == 0)
                    {
                        return josnData.Data;
                    }
                }
            }
            throw new Exception("https://ip.jiangxianli.com not allow");
        }

        public ProxyIp GetRandomIp(CountryEnum country)
        {
            List<ProxyIp> proxys = new List<ProxyIp>();
            int rint = 0;
            if (memory.Exists(MemoryEnum.Proxy.GetMemoryKey(country.GetEnumName())))
            {
                proxys = memory.Get<List<ProxyIp>>(MemoryEnum.Proxy.GetMemoryKey(country.GetEnumName()));
                if (proxys.Count > 0)
                {
                    rint = random.GetRandomInt(0, proxys.Count);
                    return proxys[rint];
                }
            }
            proxys = GetRandomIps(country);
            rint = random.GetRandomInt(0, proxys.Count);
            if (proxys.Count > 0)
            {
                rint = random.GetRandomInt(0, proxys.Count);
                return proxys[rint];
            }
            throw new Exception("https://ip.jiangxianli.com not allow");
        }
    }
}