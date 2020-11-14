using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Builder;
using Autofac.Extras.DynamicProxy;
using Autofac.Features.Scanning;
using Microsoft.AspNetCore.Mvc;

namespace Common.Utility.Extensions
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
        #endregion Public Methods
    }
}
