using Common.Utility.Random.Num;
using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using Common.Utility.Extensions.HttpClient;
using Common.Utility.HttpRequest;
using Utilities;

namespace Common.Console2
{
    internal class Program
    {
        #region Private Methods

        private static void Main(string[] args)
        {
            HttpClient2 httpClient2=new HttpClient2(randomProxy:true);
            string url = "http://httpbin.org/get";
            while (true)
            {
                var res = httpClient2.GetAsync(url).ReadString(out var statusText);
                Console.WriteLine(res);
            }
        }

        #endregion Private Methods
    }
}