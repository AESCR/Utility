﻿﻿using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Common.Utility.JwtBearer
{
    public static class AuthenticationBuilderExtensions
    {
        /// <summary>
        ///     添加Jwt授权
        /// </summary>
        /// <param name="build">
        ///     <see cref="AuthenticationBuilder" />
        /// </param>
        public static AuthenticationBuilder AddJwtBearerDefault(this AuthenticationBuilder build)
        {
            return build.AddJwtBearer(new AccessTokenOptions());
        }

        /// <summary>
        ///     添加Jwt授权
        /// </summary>
        /// <param name="build">
        ///     <see cref="AuthenticationBuilder" />
        /// </param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddJwtBearer(this AuthenticationBuilder build,
            Action<AccessTokenOptions> action)
        {
            var options = new AccessTokenOptions();
            action?.Invoke(options);
            return build.AddJwtBearer(options);
        }

        private static AuthenticationBuilder AddJwtBearer(this AuthenticationBuilder build, AccessTokenOptions options)
        {
            build.Services.AddSingleton<IAccessTokenGenerate>(new AccessTokenGenerate(options));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = options.SigningCredentials.Key,
                ValidateIssuer = true,
                ValidIssuer = options.Issuer,
                ValidateAudience = true,
                ValidAudience = options.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true
            };
            build.AddJwtBearer(options.DefaultScheme, opt =>
            {
                opt.RequireHttpsMetadata = options.RequireHttpsMetadata;
                opt.TokenValidationParameters = tokenValidationParameters;
            });
            return build;
        }
        public static void AddJwtAuth(this IServiceCollection services, Action<AccessTokenOptions> configAction)
        {
            var options = new AccessTokenOptions();
            configAction(options);
            services.AddSingleton(options);
            services.AddSingleton<IAccessTokenGenerate>(new AccessTokenGenerate(options));
        }
    }
}