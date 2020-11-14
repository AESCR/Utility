using System;
using System.Collections.Generic;
using System.Text;
using Autofac.Extras.DynamicProxy;
using Common.Utility.AOP;

namespace Common.Service
{
    [Intercept(typeof(CacheInterceptor))]
    public interface ITestService
    {
        void TestEroor();
    }

    public class TestService : ITestService
    {
        public void TestEroor()
        {
            int x = 0;
            var y = 2 / x;
        }
    }
}
