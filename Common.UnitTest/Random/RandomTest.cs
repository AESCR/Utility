using Common.Utility.Random.Proxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.UnitTest.Random
{
    [TestClass]
    public class RandomTest
    {
        [TestMethod]
        public void RandomAProxy()
        {
            RandomProxy randomProxy = new RandomProxy();
            var x = randomProxy.GetRandomIp("中国");
            var x2 = randomProxy.GetRandomIps("中国");
        }
    }
}