using Common.Utility.Extensions.HttpClient;
using Common.Utility.HttpRequest;
using System;

namespace Common.Console2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
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