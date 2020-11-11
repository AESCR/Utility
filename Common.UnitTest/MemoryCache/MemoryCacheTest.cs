using Common.Utility.HttpRequest;
using Common.Utility.Memory;
using Common.Utility.Memory.Model;
using Common.Utility.Memory.Redis;
using Common.Utility.Random.ChineseName;
using Common.Utility.Random.Num;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text;

namespace Common.UnitTest.MemoryCache
{
    [TestClass]
    public class MemoryCacheTest
    {
        public IMemoryCache RedisCache = new RedisCache(new MemoryOptions()
        {
            Password = ".netbyydsj"
        });

        [TestMethod]
        public void TestBytes()
        {
            byte[] test = Encoding.UTF8.GetBytes("AESCR");

            var result = RedisCache.Add("TestBytes", test, true);
            Assert.IsTrue(result);
            var g = RedisCache.Get<byte[]>("TestBytes");
            var r = Encoding.UTF8.GetString(g);
            Assert.AreEqual(r, "AESCR");
        }

        [TestMethod]
        public void TestClass()
        {
            HttpClient2 httpClient2 = new HttpClient2();
            string key = "TestClass";
            var result = RedisCache.Add(key, httpClient2, true);
            Assert.IsTrue(result);
            var g = RedisCache.Get<HttpClient2>(key);
            Assert.IsNotNull(g);
        }

        [TestMethod]
        public void TestInt()
        {
            string key = "TestInt";
            var result = RedisCache.Add(key, 1, true);
            Assert.IsTrue(result);
            var g = RedisCache.Get<int>(key);
            Assert.AreEqual(g, 1);
        }

        [TestMethod]
        public void TestLeng()
        {
            IRedisCache redis = RedisCache as IRedisCache;
            var x = redis.GetClient().Keys("*");
        }

        [TestMethod]
        public void TestList()
        {
            IRedisCache redis = RedisCache as IRedisCache;
            RandomNum random = new RandomNum();
            RandomName randomName = new RandomName();
            for (int i = 0; i < 100; i++)
            {
                var x = redis.AddHash("Hash", random.GenerateCheckCodeNum(10), randomName.GetRandomName());
                Assert.IsTrue(x);
                var y = redis.AddList("List", new List<string>()
                {
                    randomName.GetRandomName(),
                    randomName.GetRandomName(),
                });
                Assert.IsTrue(y > 0);
            }
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add(random.GetRandomString(10), randomName.GetRandomName());
            redis.AddHash("Hash_DIC", dic);
        }

        [TestMethod]
        public void TestString()
        {
            string key = "TestString";
            var result = RedisCache.Add(key, "AESCR", true);
            Assert.IsTrue(result);
            var g = RedisCache.Get<string>(key);
            Assert.AreEqual(g, "AESCR");
        }
    }
}