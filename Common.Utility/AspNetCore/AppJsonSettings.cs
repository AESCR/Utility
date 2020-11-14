
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
namespace Ulink.Common.Model
{
    /// <summary>
    /// json配置管理
    /// </summary>
    public static class JsonConfigure
    {
        #region Private Fields

        private static IConfiguration _configuration;

        #endregion Private Fields

        #region Public Properties

        public static RateLimit RateLimit => _configuration.Get<RateLimit>();

        public static AppSettings Settings => _configuration.Get<AppSettings>();

        #endregion Public Properties

        #region Public Methods

        public static IConfiguration GetConfiguration()
        {
            return _configuration;
        }

        public static void Init(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        #endregion Public Methods
    }
}

public class AppSettings
{
    #region Public Properties

    public SqlConnectionType ConnectionStrings { get; set; }
    public string Cors { get; set; }
    public TencentModel Tencent { get; set; }
    public string[] Urls { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class SqlConnectionType
    {
        #region Public Properties

        public string MsSQL { get; set; }
        public string MySQL { get; set; }

        #endregion Public Properties
    }

    public class TencentModel
    {
        #region Public Properties

        public Oss GlobalOss { get; set; }

        #endregion Public Properties

        #region Public Classes

        public class Oss
        {
            #region Public Properties

            public string Appid { get; set; }
            public string Bucket { get; set; }
            public string Region { get; set; }
            public string SecretId { get; set; }
            public string SecretKey { get; set; }

            #endregion Public Properties
        }

        #endregion Public Classes
    }

    #endregion Public Classes
}

public class RateLimit
{
    #region Public Properties

    public CIpRateLimiting IpRateLimiting { get; set; }
    public CIpRateLimitPolicies IpRateLimitPolicies { get; set; }

    #endregion Public Properties

    #region 类

    public class CIpRateLimiting
    {
        #region Public Properties

        /// <summary>
        /// </summary>
        public string ClientIdHeader { get; set; }

        /// <summary>
        /// </summary>
        public List<string> ClientWhitelist { get; set; }

        /// <summary>
        /// </summary>
        public string EnableEndpointRateLimiting { get; set; }

        /// <summary>
        /// </summary>
        public List<string> EndpointWhitelist { get; set; }

        /// <summary>
        /// </summary>
        public List<GeneralRulesItem> GeneralRules { get; set; }

        /// <summary>
        /// </summary>
        public int HttpStatusCode { get; set; }

        /// <summary>
        /// </summary>
        public List<string> IpWhitelist { get; set; }

        /// <summary>
        /// </summary>
        public QuotaExceededResponse QuotaExceededResponse { get; set; }

        /// <summary>
        /// </summary>
        public string RealIpHeader { get; set; }

        /// <summary>
        /// </summary>
        public string StackBlockedRequests { get; set; }

        #endregion Public Properties
    }

    public class CIpRateLimitPolicies
    {
        #region Public Properties

        /// <summary>
        /// </summary>
        public List<IpRulesItem> IpRules { get; set; }

        #endregion Public Properties
    }

    public class GeneralRulesItem
    {
        #region Public Properties

        /// <summary>
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// </summary>
        public string Period { get; set; }

        #endregion Public Properties
    }

    public class IpRulesItem
    {
        #region Public Properties

        /// <summary>
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// </summary>
        public List<RulesItem> Rules { get; set; }

        #endregion Public Properties
    }

    public class QuotaExceededResponse
    {
        #region Public Properties

        /// <summary>
        /// {{"code":429,"msg":"访问过于频繁，请稍后重试","data":null}}
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// </summary>
        public int StatusCode { get; set; }

        #endregion Public Properties
    }

    public class RulesItem
    {
        #region Public Properties

        /// <summary>
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// </summary>
        public string Period { get; set; }

        #endregion Public Properties
    }

    #endregion 类
}