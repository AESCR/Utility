using Autofac;
using Common.Utility.MemoryCache.MemoryCache2;
using Common.Utility.MemoryCache.Model;
using Common.Utility.MemoryCache.Redis;
using System;

namespace Common.Utility.Memory
{
    public interface IMemoryCache
    {
        #region Public Properties

        /// <summary>
        /// 是否是Redis
        /// </summary>
        public bool IsRedis => !(this is IMemoryCache2) && this is IRedisCache;

        #endregion Public Properties

        #region Public Methods

        bool Add<T>(string key, T value);

        bool Add<T>(string key, T value, bool isOverride);

        bool Add<T>(string key, T value, TimeSpan expires);

        bool Exists(string key);

        T Get<T>(string key);

        string GetString(string key);

        bool Remove(string key);

        bool Replace<T>(string key, T value);

        bool Replace<T>(string key, T value, TimeSpan timeSpan);

        #endregion Public Methods
    }

    public static class CacheExtensions
    {
        #region Public Methods

        public static void RegisterMemoryCache(this ContainerBuilder @this, Action<MemoryOptions> option)
        {
            //内存注入
            @this.RegisterType<Microsoft.Extensions.Caching.Memory.MemoryCache>().AsImplementedInterfaces()
                .SingleInstance();
            @this.RegisterType<MemoryCache2>().AsImplementedInterfaces().SingleInstance();
            var opt = new MemoryOptions();
            option(opt);
            var redis = @this.RegisterType<RedisCache>().AsImplementedInterfaces()
                .WithParameter(new TypedParameter(typeof(MemoryOptions), opt));
            if (opt.UseRedis == false)
            {
                redis.PreserveExistingDefaults(); //非默认值。
            }
        }

        #endregion Public Methods
    }
}