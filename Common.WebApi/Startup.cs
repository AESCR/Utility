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
    /// ����Web
    /// </summary>
    public class Startup : AutofacContainer
    {
      /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="configuration"> </param>
        /// <param name="env"> </param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env) : base(configuration, env) { }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="app"> </param>
        public override  void AppConfigure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            Console.WriteLine("����������");
        }


        /// <summary>
        /// Autofacע��
        /// </summary>
        /// <param name="builder"> </param>
        public override void AutofacInject(ContainerBuilder builder)
        {
            builder.AutoInjection(Assembly.Load("Common.Service"), typeof(CacheInterceptor));
        }

        /// <summary>
        /// IServiceCollection ע��
        /// </summary>
        /// <param name="services"> </param>
        public override void InjectServices(IServiceCollection services)
        {
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
    }
}