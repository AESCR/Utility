using Autofac;
using Common.Utility.AOP;
using Common.Utility.Autofac;
using Common.Utility.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;
using Common.Utility.AOP.Interceptor;

namespace Common.WebApi
{
    /// <summary>
    /// 启动Web
    /// </summary>
    public class Startup : AutofacContainer
    {
      /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"> </param>
        /// <param name="env"> </param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env) : base(configuration, env) { }

        /// <summary>
        /// 程序配置
        /// </summary>
        /// <param name="app"> </param>
        public override  void AppConfigure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            Console.WriteLine("程序已启动");
        }


        /// <summary>
        /// Autofac注入
        /// </summary>
        /// <param name="builder"> </param>
        public override void AutofacInject(ContainerBuilder builder)
        {
            builder.AutoInjection(Assembly.Load("Common.Service"), typeof(CacheInterceptor));
        }

        /// <summary>
        /// IServiceCollection 注入
        /// </summary>
        /// <param name="services"> </param>
        public override void InjectServices(IServiceCollection services)
        {
            services.AddControllers(o =>
                {
                    //路由前缀
                    o.UseCentralRoutePrefix(new RouteAttribute("api"));
                    o.RespectBrowserAcceptHeader = true; //浏览器和内容协商
                })
                .AddNewtonsoftJson(options =>
                {
                    //Microsoft.AspNetCore.Mvc.NewtonsoftJson
                    // 客户端请求头设置为request.Accept = "application/xml"; 返回xml数据。
                    //客户端请求头设置为request.Accept = "application/json";返回json数据。
                    // Use the default property (Pascal) casing
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                }) // 支持 NewtonsoftJson
                .AddXmlDataContractSerializerFormatters(); //添加 XML 格式支持  AddXmlDataContractSerializerFormatters可以将匿名属性式化为xml，而AddXmlSerializerFormatters直接会忽略dynamic/object类型的属性。
        }
    }
}