﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Common.Utility.Memory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Common.Utility.JwtBearer
{
    public static class JwtAuthorizeExtensions
    {
        #region Public Methods

        /// <summary>
        ///     扩展方法，对IApplicationBuilder进行扩展
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseJwtAuthorize(this IApplicationBuilder builder)
        {
            // UseMiddleware<T>
            return builder.UseMiddleware<JwtAuthorizeMiddleware>();
        }

        #endregion Public Methods
    }
    /// <summary>
    /// Jwt中间件
    /// </summary>
    public class JwtAuthorizeMiddleware
    {
        #region Private Fields


        private readonly IMemoryCache _memory;

        // 私有字段
        private readonly RequestDelegate _next;
        private readonly IAccessTokenGenerate _generate;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        ///     公共构造函数，参数是RequestDelegate类型
        ///     通过构造函数进行注入，依赖注入服务会自动完成注入
        /// </summary>
        /// <param name="next"></param>
        /// <param name="memory"></param>
        /// <param name="generate"></param>
        public JwtAuthorizeMiddleware(RequestDelegate next, IMemoryCache memory,IAccessTokenGenerate generate)
        {
            _next = next;
            _memory = memory;
            _generate = generate;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task Invoke(HttpContext context)
        {
            //获取上传token，可自定义扩展
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()
                        ?? context.Request.Headers["X-Token"].FirstOrDefault()
                        ?? context.Request.Query["Token"].FirstOrDefault()
                        ?? context.Request.Cookies["Token"];
            JwtDyUser jwtDyUser = null;
            if (token != null)
                AttachUserToContext(context, token,out jwtDyUser);

            await _next(context);
            if (jwtDyUser!=null)
            {
                //生产新的Token
                var accessToken= _generate.Generate(jwtDyUser);
                context.Response.Cookies.Append("Token",accessToken.Token);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void AttachUserToContext(HttpContext context, string token,out JwtDyUser jwtDyUser)
        {
            jwtDyUser = null;
            if ( _generate.ValidateToken(token, out var jwtToken))//认证通过
            {
                var user = JsonConvert.DeserializeObject<JwtDyUser>(jwtToken.Claims.First(x => x.Type == "user").Value);
                if (DateTime.UtcNow.Subtract(jwtToken.ValidTo).Minutes<3)
                {
                    jwtDyUser = user;
                }
                //写入认证信息，方便业务类使用
                var claimsIdentity = new ClaimsIdentity(new[]
                    {new Claim("user", jwtToken.Claims.First(x => x.Type == "user").Value)});
                Thread.CurrentPrincipal = new ClaimsPrincipal(claimsIdentity);
                context.Items["User"] = user;
            }
            else
            {
                Console.WriteLine("认证失败！");
            }
        }

        #endregion Private Methods
    }
}