using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using Common.Utility.LogDb;
using Common.Utility.Random.ChineseName;

namespace Common.Benchmarks.LogRedis
{
    [HtmlExporter]
    [MemoryDiagnoser]//要显示GC和内存分配
    public class LogRedisTest
    {
        private Utility.LogDb.LogRedis logRedis;
        private RandomName random;
        public LogRedisTest()
        {
              logRedis=new Utility.LogDb.LogRedis(new LogRedisgOptions()
            {
                Password = ".netbyydsj",
                DbIndex = 3
            });
               random=new RandomName();
        }
        [Benchmark]
        public void TestRedisA()
        {
            logRedis.Info(random.GetRandomName());
            logRedis.Warn(random.GetRandomName());
            logRedis.Error(random.GetRandomName());
        }
    }
}
