using System;
using Autofac;
using Common.Utility.Memory.MemoryCache;
using Common.Utility.MemoryCache;
using Common.Utility.MemoryCache.Options;
using Common.Utility.MemoryCache.Redis;

namespace Common.Utility.Memory
{
    public static class CacheExtensions
    {
        public static void RegisterMemoryCache(this ContainerBuilder @this, Action<MemoryOptions> option)
        {
            //内存注入
            @this.RegisterType<Microsoft.Extensions.Caching.Memory.MemoryCache>().AsImplementedInterfaces()
                .SingleInstance();
            @this.RegisterType<MemoryCache2>().AsImplementedInterfaces().SingleInstance();
            var opt = new MemoryOptions();
            option(opt);
            if (opt.UseRedis)
            {
                @this.RegisterType<RedisCache>().AsImplementedInterfaces()
                    .WithParameter(new TypedParameter(typeof(MemoryOptions), opt));
            }

        }
    }
    public interface IMemoryCache
    {
        /// <summary>
        /// 是否是Redis
        /// </summary>
        public bool IsRedis => !(this is IMemoryCache2) && this is IRedisCache;
        /// <summary>
        /// 添加缓存 key->value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expires">缓存时间</param>
        /// <returns></returns>
        bool Add(string key, object value, TimeSpan expires);
        /// <summary>
        /// 添加缓存 key->value 缓存时间不限制
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Add(string key, object value,bool isOverride = false);
        /// <summary>
        /// 是否存在缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Exists(string key);
        /// <summary>
        /// 获取缓存 转对象
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>

        T Get<T>(string key) where T : class;
        /// <summary>
        /// 获取缓存 string 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object Get(string key);

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Remove(string key);
        /// <summary>
        /// 修改缓存 string 类型
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        bool Replace(string key, object obj, TimeSpan timeSpan);
        /// <summary>
        /// 修改缓存 时间永久
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool Replace(string key, object obj);
    }
}