using Common.Utility.Memory.Model;
using Common.Utility.Memory.Redis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Common.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        public void x()
        {
            var x = new RedisCache(new MemoryOptions()).AddHash("12", new Dictionary<string, string>(), TimeSpan.Zero);
        }
    }
}