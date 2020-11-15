using Common.Utility.Extensions.HttpClient;
using Common.Utility.HttpRequest;
using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Common.Service;
using Common.Utility.AOP;
using Common.Utility.Autofac;
using Common.Utility.Utils;
using NPOI.SS.Formula.Functions;

namespace Common.Console2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var s = Assembly.Load("Common.Service");
            var t = typeof(TestService).Assembly;
            
            var x= Assembly.Load("Common.Service").GetCustomAttributes<DependsOnAttribute>();
        }
    }
}