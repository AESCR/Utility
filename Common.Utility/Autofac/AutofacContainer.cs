
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Autofac.Features.Scanning;
using Common.Utility.AOP;
using Common.Utility.AspNetCore.Swagger;
using Common.Utility.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Ulink.Common.Model;

namespace Common.Utility.Autofac
{
    public abstract class  AutofacContainer
    {
        public AutofacContainer(IConfiguration configuration, IWebHostEnvironment env)
        {
            System.Console.WriteLine($"Current State: {env.EnvironmentName}");
            JsonConfigure.Init(configuration);
        }
        private static ILifetimeScope _autofacContainer { get;  set; }
        /// <summary>
        /// 获取控制作用域
        /// </summary>
        /// <returns></returns>
        public static ILifetimeScope GetLifetimeScope()
        {
            return _autofacContainer;
        }
        public virtual void Configure(IApplicationBuilder app)
        {
            _autofacContainer = app.ApplicationServices.GetAutofacRoot();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public  void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddSwaggerGenSetup();
            ConfigureServices(services);
        }

        /// <summary>
        /// Development 
        /// </summary>
        /// <param name="app"></param>
        public void ConfigureDevelopment(IApplicationBuilder app)
        {
            app.UseSwaggerUi();
            Configure(app);
        }

        /// This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.ReplaceController();//控制器属性注入 替换规则
            // 关闭netcore自动处理参数校验机制
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
        }
        public abstract void AutoInject(ContainerBuilder builder);
        
        /// <summary>
        /// AutoFac注入
        /// </summary>
        /// <param name="builder"> </param>
        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterController();
            builder.AutoInjection(Assembly.GetExecutingAssembly());
            AutoInject(builder);
        }
    }
  
}