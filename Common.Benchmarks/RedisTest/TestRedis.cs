using BenchmarkDotNet.Attributes;
using Common.Utility.Memory.Cache;
using Common.Utility.Memory.Model;
using Common.Utility.Memory.Redis;
using Common.Utility.Random.Num;

namespace Common.Benchmarks.RedisTest
{
    /// <summary>
    /// 内存测试
    /// </summary>
    // [MarkdownExporter, AsciiDocExporter, HtmlExporter, CsvExporter, RPlotExporter]
    [HtmlExporter]
    [MemoryDiagnoser]//要显示GC和内存分配
    public class TestRedis
    {
        private readonly IRedisCache _redisCache;
        private readonly IMemoryCache2 memoryCache2;
        private RandomNum random = new RandomNum();
        [Params(10, 100, 1000)] public int Time;

        public TestRedis()
        {
            _redisCache = new RedisCache(new MemoryOptions()
            {
                Password = ".netbyydsj"
            });
            memoryCache2 = new MemoryCache2();
        }

        [Benchmark]
        public void TestMemoryCacheB()
        {
            var key = random.GenerateCheckCodeNum(10);
            memoryCache2.Add(key, "AESCR");
        }

        [Benchmark]
        public void TestRedisA()
        {
            var key = random.GenerateCheckCodeNum(10);
            _redisCache.Add(key, "AESCR");
        }
    }
}