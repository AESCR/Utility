using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac;
using Castle.DynamicProxy;
using Common.Utility.Autofac;

namespace Common.Utility.AOP.Interceptor
{
    public class BaseIInterceptor:IInterceptor, ITransientDependency
    {
        public void Intercept(IInvocation invocation)
        {
            var attrs = invocation.Method.GetCustomAttribute<InterceptMethodAttribute>();
            if (attrs==null)
            {
                invocation.Proceed();
            }
            else
            {
                using (var lifetime = AutofacContainer.GetLifetimeScope().BeginLifetimeScope())
                {
                    foreach (Type type in attrs.InterceptTypes)
                    {
                        var o = lifetime.Resolve(type);
                        if (o is IAsyncInterceptor asyncO)
                        {
                            asyncO.ToInterceptor().Intercept(invocation);
                        }
                        else if (o is IInterceptor syncO)
                        {
                            syncO.Intercept(invocation);
                        }
                        else
                        {
                            lifetime.Dispose();
                            throw new Exception($"InterceptMethodAttribute InterceptTypes not found {type.FullName}");
                        }
                    }
                }
            }
        }
    }
}
