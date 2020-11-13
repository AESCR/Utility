using System.Net;
using System.Text;
using System.Threading.Tasks;
using Common.Utility.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Common.Utility.AspNetCore.Middleware
{
    public class SecurityMiddleware
    {
        // 私有字段
        private readonly RequestDelegate _next;
        public SecurityMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //HTTP Referer 防范CSRF 防jsONP劫持、盗链、站外提交
            if (!context.Request.IsLocal())
            {
                context.Response.StatusCode = (int) HttpStatusCode.MethodNotAllowed;
                context.Response.Body.Write(Encoding.UTF8.GetBytes("跨站请求不被允许!请求被拦截"));
                return;
            }
            await _next.Invoke(context);
        }
    }
    /// <summary>
    /// 扩展
    /// </summary>
    public static class SecurityeMiddlewareExtensions
    {
        public static IApplicationBuilder UsSecurityeMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SecurityMiddleware>();
        }
    }
}
