using System;
using Common.Utility.Extensions.System;
using Common.Utility.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.UnitTest.Utils
{
    [TestClass]
    public class QueryUtilsTest
    {
[TestMethod]
        public void GetPublicIp()
        {
            var x= QueryUtils.PublicIp();
            Console.WriteLine(x);
            if (x.IsNullOrEmpty())
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void QueryIpAddress()
        {
            var x= QueryUtils.IpAddress("175.141.69.200");
            Console.WriteLine(x);
            if (x.IsNullOrEmpty())
            {
                Assert.Fail();
            }
        }
    }
}