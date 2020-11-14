
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Extras.DynamicProxy;
using Autofac.Features.Scanning;
using Common.Utility.AOP;
using Common.Utility.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Utility.Autofac
{
    public abstract class AutofacContainer
    {
        public abstract void AutoInject(ContainerBuilder builder);

        /// <summary>
        /// AutoFac注入
        /// </summary>
        /// <param name="builder"> </param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterController();
            AutoInject(builder);
            builder.RegisterType<CacheInterceptor>().SingleInstance().AsSelf();
        }
    }
  
}