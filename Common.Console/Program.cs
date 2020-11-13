using Common.Utility.Extensions.HttpClient;
using Common.Utility.HttpRequest;
using System;
using Common.Utility.Utils;

namespace Common.Console2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string str = "I am {Name},Age is {Age}";
            var x=  RegexUtils.RegexValue(str, @"{\w+}");
            HttpClient2 httpClient2 = new HttpClient2(randomProxy: true);
            string url = "http://httpbin.org/get";
            while (true)
            {
                var res = httpClient2.GetAsync(url).ReadString(out var statusText);
                Console.WriteLine(res);
            }
        }
    }
}