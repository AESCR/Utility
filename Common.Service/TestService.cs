using System;
using System.Collections.Generic;
using System.Text;
using Autofac.Extras.DynamicProxy;
using Common.Utility.AOP;
using Common.Utility.Autofac;
using Common.Utility.Random.ChineseName;

namespace Common.Service
{
    public interface ITestService: ISingletonDependency, IAutoInterceptor
    {
        string TestOk();
        string TestEroor();
        [NoIntercept]
        string TestNoCacheOk();
    }
    public class TestService :  ITestService
    {
        public RandomName randomName { get; set; }
        public string TestOk()
        {
           return randomName.GetRandomName();
        }
        public string TestNoCacheOk()
        {
            return randomName.GetRandomName();
        }
        public string TestEroor()
        {
            int x = 0;
            var y = 2 / x;
            return "error";
        }
    }
}
