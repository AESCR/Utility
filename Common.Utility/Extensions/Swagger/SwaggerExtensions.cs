using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Common.Utility.Extensions.Swagger
{
    public static class SwaggerExtensions
    {
        /// <summary>
        /// 服务
        /// </summary>
        /// <param name="this"> </param>
        /// <param name="openAction"> </param>
        public static void AddSwaggerGen(this IServiceCollection @this, Action<OpenApiInfo> openAction)
        {
            @this.AddSwaggerGen(o =>
            {
                var openApi = new OpenApiInfo
                {
                    Title = "Document API",
                    Version = "v1",
                    Description = "ASP.NET Core Web API"
                };
                openAction(openApi);
                o.SwaggerDoc("v1", openApi
                );
                // 使用反射获取xml文件。并构造出文件的路径
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
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
            @this.UseSwagger();
            @this.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Document API V1");
                c.RoutePrefix = "swagger";
            });
        }
    }
}