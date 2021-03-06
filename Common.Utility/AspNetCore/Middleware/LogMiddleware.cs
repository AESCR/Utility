﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using NLog;
using System;
using System.Threading.Tasks;

namespace Common.Utility.Middleware
{
    /// <summary>
    /// 扩展中间件
    /// </summary>
    public static class RequestResponseLoggingMiddlewareExtensions
    {
        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="app"> </param>
        /// <returns> </returns>
        public static IApplicationBuilder UseLogRecord(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LogMiddleware>();
        }
    }

    public class LogMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public LogMiddleware(RequestDelegate next)
        {
            _next = next;
            _logger = LogManager.GetLogger("logs");
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.Info("Request Url:" + context.Request.Path + Environment.NewLine
                         + "Body:" + context.Request.Body.ToString());
            await _next.Invoke(context);
            _logger.Info("Response Url:" + context.Request.Path + Environment.NewLine
                         + "Body:" + context.Response.Body.ToString());
        }
    }
}