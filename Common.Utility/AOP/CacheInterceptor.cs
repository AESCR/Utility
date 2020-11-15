using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Common.Utility.Autofac;
using Common.Utility.Memory.Cache;

namespace Common.Utility.AOP
{
    /// <summary>
    /// 
    /// </summary>
    public class CacheInterceptor : AutofacInterceptor, ISingletonDependency
    {
        private readonly IMemoryCache2 memory=new MemoryCache2();
        public override void AutofacIntercept(IInvocation invocation)
        {
            var name = invocation.Proxy.ToString();
            var keyName = name + invocation.Method.Name + "_" + string.Join("_", invocation.Arguments);
            if (memory.Exists(keyName))
            {
                invocation.ReturnValue = memory.Get(keyName);
                return;
            }
            invocation.Proceed();
            memory.Add(keyName, invocation.ReturnValue, TimeSpan.FromSeconds(10), true);
        }

    }
}