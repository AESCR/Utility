using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Common.Utility.AspNetCore.MiniProfiler;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Common.Utility.AspNetCore.Swagger
{
    public static class SwaggerExtensions
    {
        /// <summary>
        /// 服务
        /// </summary>
        /// <param name="this"> </param>
        /// <param name="openAction"> </param>
        public static void AddSwaggerGenSetup(this IServiceCollection @this, Action<OpenApiInfo> openAction=null)
        {
            @this.AddMiniProfilerSetup();
            @this.AddSwaggerGen(o =>
            {
                var openApi = new OpenApiInfo
                {
                    Title = "Document API",
                    Version = "v1",
                    Description = "ASP.NET Core Web API"
                };
                openAction?.Invoke(openApi);
                o.SwaggerDoc("v1", openApi
                );
                // 使用反射获取xml文件。并构造出文件的路径
                var xmlFile = $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // 启用xml注释. 该方法第二个参数启用控制器的注释，默认为false.
                o.IncludeXmlComments(xmlPath, true);

                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "请输入带有Bearer的Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT"
                });
                o.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                        },
                        new List<string>()
                    }
                });
            });
        }

        /// <summary>
        /// 启动配置页面
        /// </summary>
        /// <param name="this"> </param>
        public static void UseSwaggerUi(this IApplicationBuilder @this)
        {
            @this.UseMiniProfiler();
            @this.UseSwagger();
            @this.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Document API V1");
                c.RoutePrefix = "swagger";
                c.HeadContent = @$"<script async id='mini-profiler' src='/profiler/includes.min.js?v=4.2.1+b27bea37e9' data-version='4.2.1+b27bea37e9' data-path='/profiler/' data-current-id='144b1192-acd3-4fe2-bbc5-6f1e1c6d53df' data-ids='87a1341b-995d-4d1d-aaba-8f2bfcfc9ca9,144b1192-acd3-4fe2-bbc5-6f1e1c6d53df' data-position='Left' data-scheme='Light' data-authorized='true' data-max-traces='15' data-toggle-shortcut='Alt+P' data-trivial-milliseconds='2.0' data-ignored-duplicate-execute-types='Open,OpenAsync,Close,CloseAsync'></script>";
                c.IndexStream = () =>
                    Assembly.GetExecutingAssembly().GetManifestResourceStream("Common.Utility.AspNetCore.MiniProfiler.index.html");
            });
        }
    }
}