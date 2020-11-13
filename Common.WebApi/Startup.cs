using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace Common.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        /// <summary>
        /// AutoFac注入
        /// </summary>
        /// <param name="builder"> </param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(o =>
                {
                    //路由前缀
                    o.UseCentralRoutePrefix(new RouteAttribute("api"));
                    o.RespectBrowserAcceptHeader = true; //浏览器和内容协商
                    GlobalFilters.AddFilter(o.Filters);
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

            // 关闭netcore自动处理参数校验机制
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
        }
    }
}