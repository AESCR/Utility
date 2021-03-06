﻿using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Common.Utility.Autofac;
using Common.Utility.Memory.Cache;

namespace Common.Utility.AOP.Interceptor
{
    /// <summary>
    /// [Intercept(typeof(CacheInterceptor))]
    /// </summary>
    public class CacheInterceptor : AutofacInterceptor, ITransientDependency
    {
        public CacheAsyncInterceptor AsyncInterceptor { get; set; }
        public override void SyncIntercept(IInvocation invocation)
        {
            Console.WriteLine(AsyncInterceptor.GetHashCode());
            AsyncInterceptor.ToInterceptor().Intercept(invocation);
        }
    }

    public class CacheAsyncInterceptor : AutofacAsyncInterceptor, ISingletonDependency
    {
        private readonly IMemoryCache2 _memory = new MemoryCache2();
        public override void SyncIntercept(IInvocation invocation)
        {
            var name = invocation.Proxy.ToString();
            var keyName = name + invocation.Method.Name + "_" + string.Join("_", invocation.Arguments);
            if (_memory.Exists(keyName))
            {
                invocation.ReturnValue = _memory.Get(keyName);
                return;
            }
            invocation.Proceed();
            _memory.Add(keyName, invocation.ReturnValue, TimeSpan.FromSeconds(10), true);
        }

        public override void AsynIntercept(IInvocation invocation)
        {
            invocation.Proceed();
        }

        public override void AsynInterceptResult<TResult>(IInvocation invocation)
        {
            invocation.ReturnValue = InternalInterceptAsynchronous<TResult>(invocation);
        }
        private async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
        {
            var name = invocation.Proxy.ToString();
            var keyName = name + invocation.Method.Name + "_" + string.Join("_", invocation.Arguments);
            if (_memory.Exists(keyName))
            {
                invocation.ReturnValue = _memory.Get(keyName);
                return await Task.FromResult((TResult)invocation.ReturnValue);
            }
            invocation.Proceed();
            var task = (Task<TResult>)invocation.ReturnValue;
            TResult result = await task;
            _memory.Add(keyName, result, TimeSpan.FromSeconds(10), true);
            //记录日志
            Console.WriteLine($"{invocation.Method.Name} 已执行，返回结果：{result}");
            return result;
        }
    }
}