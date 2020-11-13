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
        /// AutoFacע��
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
                    //·��ǰ׺
                    o.UseCentralRoutePrefix(new RouteAttribute("api"));
                    o.RespectBrowserAcceptHeader = true; //�����������Э��
                    GlobalFilters.AddFilter(o.Filters);
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

            // �ر�netcore�Զ��������У�����
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
        }
    }
}