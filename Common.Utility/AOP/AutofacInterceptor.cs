﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Castle.DynamicProxy;

namespace Common.Utility.AOP
{
    public abstract class AutofacAsyncInterceptor : IAsyncInterceptor
    {
        public abstract void SyncIntercept(IInvocation invocation);
        public abstract void AsynIntercept(IInvocation invocation);
        public abstract void AsynInterceptResult<TResult>(IInvocation invocation);

        public void InterceptSynchronous(IInvocation invocation)
        {
            SyncIntercept(invocation);
        }

        public void InterceptAsynchronous(IInvocation invocation)
        {
            AsynIntercept(invocation);
        }

        public void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            AsynInterceptResult<TResult>(invocation);
        }

       
    }
    public abstract class AutofacInterceptor : IInterceptor
    {
        public abstract void SyncIntercept(IInvocation invocation);
        public void Intercept(IInvocation invocation)
        {
            if (invocation.IsIntercept(this))
            {
                SyncIntercept(invocation);
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
    public static class InvocationExtensions
    {
        /// <summary>
        /// 是否满足拦截 
        /// </summary>
        /// <param name="invocation"></param>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsIntercept(this IInvocation invocation,object @this)
        {
            var noAttr = invocation.Method.GetCustomAttribute<NoInterceptAttribute>();
            if (noAttr == null) return true;
            if (noAttr.InterceptTypes.Length != 0)
            {
                return !noAttr.InterceptTypes.Contains(@this.GetType());
            }
            return false;

        }
    }
}
