﻿using Autofac;
using Common.Utility.Memory.Cache;
using Common.Utility.Memory.Model;
using Common.Utility.Memory.Redis;
using System;

namespace Common.Utility.Memory
{
    public interface IMemoryCache
    {
        /// <summary>
        /// 是否是Redis
        /// </summary>
        public bool IsRedis => !(this is IMemoryCache2) && this is IRedisCache;

        bool Add<T>(string key, T value);

        bool Add<T>(string key, T value, bool isOverride);

        bool Add<T>(string key, T value, TimeSpan expires);

        bool Exists(string key);

        T Get<T>(string key);

        string GetString(string key);

        bool Remove(string key);

        bool Replace<T>(string key, T value);

        bool Replace<T>(string key, T value, TimeSpan timeSpan);
    }

    public static class CacheExtensions
    {
        /*public static void RegisterCache(this ContainerBuilder @this, Action<MemoryOptions> option=null)
        {
            //内存注入
            /* @this.RegisterType<Microsoft.Extensions.Caching.Memory.MemoryCache>().AsImplementedInterfaces()
                 .SingleInstance();#1#
            @this.RegisterType<MemoryCache2>().AsImplementedInterfaces().SingleInstance();
            var opt = new MemoryOptions();
            if (option == null) return;
            option(opt);
            var redis = @this.Register((c, p) =>
                new RedisCache(option)).AsImplementedInterfaces();
            if (opt.UseRedis == false)
            {
                redis.PreserveExistingDefaults(); //非默认值。
            }
        }*/
        public static void RegisterRedis(this ContainerBuilder @this, Action<MemoryOptions> option)
        {
            var opt = new MemoryOptions();
            option(opt);
            var redis = @this.Register((c, p) =>
                    new RedisCache(option)).AsImplementedInterfaces().PropertiesAutowired().InstancePerLifetimeScope()
                .OnRelease(e => (e as IDisposable)?.Dispose());
            if (opt.UseRedis == false)
            {
                redis.PreserveExistingDefaults(); //非默认值。
            }
        }
    }
}