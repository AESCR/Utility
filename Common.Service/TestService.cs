using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Extras.DynamicProxy;
using Common.Utility.AOP;
using Common.Utility.AOP.Interceptor;
using Common.Utility.Autofac;
using Common.Utility.Random.ChineseName;

namespace Common.Service
{
    public interface ITestService: ISingletonDependency, IAutoInterceptor
    {
     
        Task<string> TestOk();
        [NoIntercept]
        Task<string> TestEroor();
        [NoIntercept(typeof(CacheInterceptor))]
        Task<string> TestNoCacheOk();
    }
    public class TestService :  ITestService
    {
        public RandomName randomName { get; set; }
        public Task<string> TestOk()
        {
            return Task.Run(() =>
            {
                return randomName.GetRandomName();
            });
        }
        public Task<string> TestNoCacheOk()
        {
            return Task.Run(() =>
            {
                return randomName.GetRandomName();
            });
        }
        public Task<string> TestEroor()
        {
            return Task.Run(() =>
            {
                int x = 0;
                var y = 2 / x;
                return "error";
            });
        }
    }
}
