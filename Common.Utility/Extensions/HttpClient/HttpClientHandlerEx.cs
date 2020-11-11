using System;
using System.Net;
using System.Net.Http;

namespace Common.Utility.Extensions.HttpClient
{
    public static class HttpClientHandlerEx
    {
        /// <summary>
        /// Use Default
        /// </summary>
        /// <returns> </returns>
        public static HttpClientHandler UseDefault(this HttpClientHandler @this)
        {
            @this.ServerCertificateCustomValidationCallback = (message, cert, chain, error) => true;
            @this.AllowAutoRedirect = true;
            @this.AutomaticDecompression = DecompressionMethods.GZip;
            @this.UseCookies = true;
            return @this;
        }

        /// <summary>
        /// 设置代理
        /// </summary>
        /// <param name="httpClientHandler"> </param>
        /// <param name="urlString"> </param>
        public static HttpClientHandler UseWebProxy(this HttpClientHandler httpClientHandler, string urlString)
        {
            httpClientHandler.UseProxy = true;
            var wp = new WebProxy();
            wp.Address = new Uri(urlString);
            httpClientHandler.Proxy = wp;
            return httpClientHandler;
        }
    }
}