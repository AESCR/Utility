using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common.Utility.AspNetCore.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using Ulink.Common.Model;

namespace Common.Utility.Autofac
{


    public abstract class AutofacContainer
    {
        protected AutofacContainer(IConfiguration configuration, IWebHostEnvironment env)
        {
            Console.WriteLine($"Current State: {env.EnvironmentName}");
            JsonConfigure.Init(configuration);
        }

        public abstract void AppConfigure(IApplicationBuilder app);
        public abstract void AutofacInject(ContainerBuilder builder);
        public abstract void InjectServices(IServiceCollection services);
        private static ILifetimeScope LifetimeScope { get; set; }

        /// <summary>
        /// 获取控制作用域
        /// </summary>
        /// <returns> </returns>
        public static ILifetimeScope GetLifetimeScope()
        {
            return LifetimeScope;
        }

        /// <summary>
        /// 程序配置
        /// </summary>
        /// <param name="app"></param>
        public void Configure(IApplicationBuilder app)
        {
           
            LifetimeScope = app.ApplicationServices.GetAutofacRoot();
            AppConfigure(app);
        }

        /// <summary>
        /// AutoFac注入
        /// </summary>
        /// <param name="builder"> </param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterController();
            builder.AutoInjection(Assembly.GetExecutingAssembly());
            AutofacInject(builder);
        }

        /// <summary>
        /// Development程序配置
        /// </summary>
        /// <param name="app"> </param>
        public void ConfigureDevelopment(IApplicationBuilder app)
        {
            app.UseSwaggerUi();
            Configure(app);
        }

        /// <summary>
        /// 测试环境配置
        /// </summary>
        /// <param name="services"> </param>
        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddSwaggerGenSetup();
            ConfigureServices(services);
        }

        /// <summary>
        /// IServiceCollection 注入
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.ReplaceController(); //控制器属性注入 替换规则
            // 关闭netcore自动处理参数校验机制
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
            InjectServices(services);
        }
    }

}