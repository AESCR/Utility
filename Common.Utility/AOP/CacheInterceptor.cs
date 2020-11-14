using System.Collections.Generic;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Common.Utility.AOP
{
    /// <summary>
    /// 
    /// </summary>
    public class CacheInterceptor : IInterceptor
    {
        private readonly Dictionary<string, object> _cacheDict = new Dictionary<string, object>();

        public  void  Intercept(IInvocation invocation)
        {
            var name = invocation.Proxy.ToString();
            var keyName = invocation.Method.Name + "_" + string.Join("_", invocation.Arguments);
            if (_cacheDict.ContainsKey(keyName))
            {
                invocation.ReturnValue = _cacheDict[keyName];
                return;
            }
            invocation.Proceed();
            _cacheDict[keyName] = "cache:" + invocation.ReturnValue;
        }
    }
}