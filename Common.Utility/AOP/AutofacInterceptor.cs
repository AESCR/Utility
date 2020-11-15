using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Common.Utility.AOP
{
    public abstract class  AutofacInterceptor: IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var noAttr= invocation.Method.GetCustomAttribute<NoInterceptAttribute>();
            if (noAttr!=null)
            {
                if (noAttr.InterceptTypes.Length==0)
                {
                    invocation.Proceed();
                    return;
                }
                if (noAttr.InterceptTypes.Contains(GetType()))
                {
                    invocation.Proceed();
                    return;
                }
            }
            //拦截
            AutofacIntercept(invocation);
        }

        public abstract void AutofacIntercept(IInvocation invocation);
    }
}
