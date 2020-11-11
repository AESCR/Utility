using Common.Utility.HttpResponse;
using Common.Utility.Memory;
using Common.Utility.Memory.Model;
using Common.Utility.Memory.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utility.Middleware
{
    /// <summary>
    /// 黑名单管理
    /// </summary>
    public static class IpBlackUtils
    {
        public static bool AddIpBlack(IMemoryCache memory, string ip)
        {
            if (memory.IsRedis)
            {
                IRedisCache redis = (IRedisCache)memory;
                return redis.AddList(MemoryEnum.BlackIps.GetMemoryKey(), ip) > 0;
            }
            IMemoryCache memoryCache = (IMemoryCache)memory;
            var black = memoryCache.Get<List<string>>(MemoryEnum.BlackIps.GetMemoryKey());
            black.Add(ip);
            return memoryCache.Add(MemoryEnum.BlackIps.GetMemoryKey(), black, true);
        }

        public static bool Clear(IMemoryCache memory)
        {
            return memory.Remove(MemoryEnum.BlackIps.GetMemoryKey());
        }

        public static string[] GetIpBlacks(IMemoryCache memory)
        {
            if (memory.IsRedis)
            {
                IRedisCache redis = (IRedisCache)memory;
                return redis.GetCollection(MemoryEnum.BlackIps.GetMemoryKey());
            }
            else
            {
                var black = memory.Get<List<string>>(MemoryEnum.BlackIps.GetMemoryKey());
                return black.ToArray();
            }
        }

        public static bool RemoveIpBlack(IMemoryCache memory, string ip)
        {
            if (memory.IsRedis)
            {
                IRedisCache redis = (IRedisCache)memory;
                return redis.DelList(MemoryEnum.BlackIps.GetMemoryKey(), ip) > 0;
            }
            IMemoryCache memoryCache = (IMemoryCache)memory;
            var black = memoryCache.Get<List<string>>(MemoryEnum.BlackIps.GetMemoryKey());
            black.Remove(ip);
            return memoryCache.Add(MemoryEnum.BlackIps.GetMemoryKey(), black, true);
        }
    }

    public static class RequestIpMiddlewareExtensions
    {
        /// <summary>
        /// IP黑名单 IpBlackUtils.AddIpBlack() 操作黑名单
        /// </summary>
        /// <param name="builder"> </param>
        /// <returns> </returns>
        public static IApplicationBuilder UseRequestIpMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestIpMiddleware>();
        }
    }

    /// <summary>
    /// 登录IP记录
    /// </summary>
    public class RequestIpMiddleware
    {
        private readonly IMemoryCache _memory;
        private readonly RequestDelegate _next;

        public RequestIpMiddleware(RequestDelegate next, IMemoryCache memory)
        {
            _next = next;
            _memory = memory;
        }

        public async Task Invoke(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (_memory.Exists(MemoryEnum.BlackIps.GetMemoryKey()))
            {
                string[] blackish;
                if (_memory.IsRedis)
                {
                    IRedisCache redis = (IRedisCache)_memory;
                    blackish = redis.GetCollection(MemoryEnum.BlackIps.GetMemoryKey());
                }
                else
                {
                    blackish = _memory.Get<string[]>(MemoryEnum.BlackIps.GetMemoryKey());
                }
                if (blackish != null && blackish.Contains(ip))
                {
                    //拒绝
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    var response = ResponseUtils.GetResponseByCode((int)SysCode.IPAccessDenied);
                    var json = JsonConvert.SerializeObject(response);
                    byte[] data = Encoding.UTF8.GetBytes(json);
                    context.Response.ContentType = "application/json";
                    context.Response.ContentLength = data.Length;
                    await context.Response.Body.WriteAsync(data, 0, data.Length);
                    return;
                }
            }
            await _next.Invoke(context);
        }
    }
}