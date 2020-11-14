
using Autofac;
using Common.Utility.Autofac;
using Common.Utility.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Serialization;
using System;
using Autofac.Extensions.DependencyInjection;
using Common.Utility.AOP;
using Common.Utility.AspNetCore.MiniProfiler;
using Common.Utility.AspNetCore.Swagger;
using Microsoft.AspNetCore.Http;
using Ulink.Common.Model;

namespace Common.WebApi
{
    public class Startup :AutofacContainer
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            System.Console.WriteLine($"Current State: {env.EnvironmentName}");
            JsonConfigure.Init(configuration);
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        // Development环境下执行的ConfigureServices方法
        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddSwaggerGenSetup();
            ConfigureServices(services);
        }

        // Development环境下执行的Configure方法
        public void ConfigureDevelopment(IApplicationBuilder app)
        {
            app.UseSwaggerUi();
            Configure(app);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(o =>
                {
                    //路由前缀
                    o.UseCentralRoutePrefix(new RouteAttribute("api"));
                    o.RespectBrowserAcceptHeader = true; //浏览器和内容协商
                })
                .AddControllersAsServices()
                .AddNewtonsoftJson(options =>
                {
                    //Microsoft.AspNetCore.Mvc.NewtonsoftJson
                    // 客户端请求头设置为request.Accept = "application/xml"; 返回xml数据。
                    //客户端请求头设置为request.Accept = "application/json";返回json数据。
                    // Use the default property (Pascal) casing
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                }) // 支持 NewtonsoftJson
                .AddXmlDataContractSerializerFormatters(); //添加 XML 格式支持  AddXmlDataContractSerializerFormatters可以将匿名属性式化为xml，而AddXmlSerializerFormatters直接会忽略dynamic/object类型的属性。

            // 关闭netcore自动处理参数校验机制
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
        }

        public override void AutoInject(ContainerBuilder builder)
        {
            builder.RegisterServices("Common.Service");
        }
    }
}