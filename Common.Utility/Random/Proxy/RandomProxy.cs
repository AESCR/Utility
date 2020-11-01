using Common.Utility.Extensions.HttpClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Common.Utility.Autofac;
using Common.Utility.Utils;
using COSXML.Network;
using HttpClient = System.Net.Http.HttpClient;

namespace Common.Utility.Random.Proxy
{
    /// <summary>
    /// 代理IP
    /// </summary>
    public class ProxyIp
    {
        #region Public Properties

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

        #endregion Public Properties
    }

    public class ProxyResponse
    {
        #region Public Properties

        public int Code { get; set; }
        public ProxyIp Data { get; set; }
        public string Msg { get; set; }

        #endregion Public Properties
    }

    public class ProxysResponse
    {
        #region Public Properties

        public int Code { get; set; }
        public ProxyListIP Data { get; set; }
        public string Msg { get; set; }

        #endregion Public Properties

        #region Public Classes

        public class ProxyListIP
        {
            #region Public Properties

            [JsonProperty("current_page")] public string CurrentPage { get; set; }
            public List<ProxyIp> Data { get; set; }

            #endregion Public Properties
        }

        #endregion Public Classes
    }

    /// <summary>
    /// 随机代理
    /// </summary>
    public class RandomProxy: ISingletonDependency
    {
        #region Private Fields

        private HttpClient httpClient;

        #endregion Private Fields

        #region Public Constructors

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

        #endregion Public Constructors

        #region Public Methods

        public ProxyIp GetRandomIp(string country="")
        {
            try
            {
                string url = "https://ip.jiangxianli.com/api/proxy_ip";
                Dictionary<string,string> param=new Dictionary<string, string>();
                if (string.IsNullOrWhiteSpace(country)==false)
                {
                    param.Add("country",country);
                }

                url = url+"?"+HttpUtils.ParamsToUrl(param);
                var json = httpClient.DoGet(url).ReadString();
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

        public List<ProxyIp> GetRandomIps(string country = "")
        {
            try
            {
                string url = "https://ip.jiangxianli.com/api/proxy_ips";
                Dictionary<string, string> param = new Dictionary<string, string>();
                if (string.IsNullOrWhiteSpace(country) == false)
                {
                    param.Add("country", country);
                }

                url = url + "?" + HttpUtils.ParamsToUrl(param);
                var json = httpClient.DoGet(url).ReadString();
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

        #endregion Public Methods
    }
}