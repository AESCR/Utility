using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using Common.Utility.Memory;
using Common.Utility.MemoryCache.Model;
using Common.Utility.MemoryCache.Redis;
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
        [Params(10, 100, 1000, 2000)] public int Time;

        private readonly IMemoryCache _redisCache;
        RandomNum random=new RandomNum();
        public TestRedis()
        {
            _redisCache= new RedisCache(new MemoryOptions()
            {
                Password = ".netbyydsj"
            });
        }
        [Benchmark]
        public void TestRedisA()
        {
            var key= random.GenerateCheckCodeNum(10);
            _redisCache.Add(key, "AESCR");
        }
        [Benchmark]
        public void TestRedisB()
        {
            var tempCache = new RedisCache(new MemoryOptions()
            {
                Password = ".netbyydsj"
            });
            var key = random.GenerateCheckCodeNum(10);
            tempCache.Add(key, "AESCR");
        }
    }
}
