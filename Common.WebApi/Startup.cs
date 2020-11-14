
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

        // Development������ִ�е�ConfigureServices����
        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddSwaggerGenSetup();
            ConfigureServices(services);
        }

        // Development������ִ�е�Configure����
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
                    //·��ǰ׺
                    o.UseCentralRoutePrefix(new RouteAttribute("api"));
                    o.RespectBrowserAcceptHeader = true; //�����������Э��
                })
                .AddControllersAsServices()
                .AddNewtonsoftJson(options =>
                {
                    //Microsoft.AspNetCore.Mvc.NewtonsoftJson
                    // �ͻ�������ͷ����Ϊrequest.Accept = "application/xml"; ����xml���ݡ�
                    //�ͻ�������ͷ����Ϊrequest.Accept = "application/json";����json���ݡ�
                    // Use the default property (Pascal) casing
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                }) // ֧�� NewtonsoftJson
                .AddXmlDataContractSerializerFormatters(); //��� XML ��ʽ֧��  AddXmlDataContractSerializerFormatters���Խ���������ʽ��Ϊxml����AddXmlSerializerFormattersֱ�ӻ����dynamic/object���͵����ԡ�

            // �ر�netcore�Զ��������У�����
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
        }

        public override void AutoInject(ContainerBuilder builder)
        {
            builder.RegisterServices("Common.Service");
        }
    }
}