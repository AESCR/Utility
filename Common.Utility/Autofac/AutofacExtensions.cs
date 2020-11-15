using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Extras.DynamicProxy;
using Autofac.Features.Scanning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Utilities;

namespace Common.Utility.Autofac
{
    public static class AutofacExtensions
    {
        #region Public Methods

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterServices(
            this ContainerBuilder @this, string assembly, string endName = "Service")
        {
            return @this.RegisterAssemblyTypes(Assembly.Load(assembly))
                .Where(a => a.Name.EndsWith(endName))
                .PublicOnly().AsImplementedInterfaces().EnableInterfaceInterceptors();
            ;
        }
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterController(this ContainerBuilder @this)
        {
            //获取所有控制器类型并使用属性注入
            var controllerBaseType = typeof(ControllerBase);
            return @this.RegisterAssemblyTypes(Assembly.GetEntryAssembly())
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                .PropertiesAutowired();
        }
        /// <summary>
        /// 控制器属性注入
        /// </summary>
        /// <param name="services"></param>
        public static void ReplaceController(this IServiceCollection services)
        {
            services.Replace(ServiceDescriptor
                .Transient<IControllerActivator, ServiceBasedControllerActivator>());
        }
        #endregion Public Methods
    }
}
