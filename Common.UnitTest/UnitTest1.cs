using System;
using System.Collections;
using Common.Utility.JwtBearer;
using Common.Utility.MemoryCache.Redis;
using Common.Utility.SystemExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Common.Utility;
using Common.Utility.Extensions.HttpClient;
using Common.Utility.MemoryCache.Model;
using Common.Utility.Random.ChineseName;
using Common.Utility.Random.Num;
using Microsoft.Extensions.Caching.Memory;

namespace Common.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        public void x()
        {
           var x=  new RedisCache(new MemoryOptions()).AddHash("12", new Dictionary<string, string>(), TimeSpan.Zero);
        }

    }
}