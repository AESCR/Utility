
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
using System.Reflection;
using System.Text;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Common.Service;
using Common.Utility.AOP;
using Common.Utility.AspNetCore.MiniProfiler;
using Common.Utility.AspNetCore.Swagger;
using Common.Utility.Memory.Cache;
using Microsoft.AspNetCore.Http;
using Ulink.Common.Model;

namespace Common.WebApi
{
    public class Startup :AutofacContainer
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env) : base(configuration, env){}
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app)
        {
            base.Configure(app);
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            Console.WriteLine("����������");
        }
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddControllers(o =>
                {
                    //·��ǰ׺
                    o.UseCentralRoutePrefix(new RouteAttribute("api"));
                    o.RespectBrowserAcceptHeader = true; //�����������Э��
                })
                .AddNewtonsoftJson(options =>
                {
                    //Microsoft.AspNetCore.Mvc.NewtonsoftJson
                    // �ͻ�������ͷ����Ϊrequest.Accept = "application/xml"; ����xml���ݡ�
                    //�ͻ�������ͷ����Ϊrequest.Accept = "application/json";����json���ݡ�
                    // Use the default property (Pascal) casing
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                }) // ֧�� NewtonsoftJson
                .AddXmlDataContractSerializerFormatters(); //��� XML ��ʽ֧��  AddXmlDataContractSerializerFormatters���Խ���������ʽ��Ϊxml����AddXmlSerializerFormattersֱ�ӻ����dynamic/object���͵����ԡ�

        }
        /// <summary>
        /// Autofac�Զ�ע��
        /// </summary>
        /// <param name="builder"></param>

        public override void AutoInject(ContainerBuilder builder)
        {
            builder.AutoInjection(Assembly.Load("Common.Service"),typeof(CacheInterceptor));
        }
    }
}