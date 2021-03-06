using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using Common.Utility.Autofac;

namespace Common.Utility.Memory.Cache
{
    public class MemoryCache2 :  IMemoryCache2
    {

        /// <summary>
        /// MemoryCache 缓存
        /// </summary>
        private  Microsoft.Extensions.Caching.Memory.IMemoryCache _cache;

        /// <summary>
        /// 构造器注入 IMemoryCache
        /// </summary>
        public MemoryCache2()
        {
            var options = new MemoryCacheOptions();
            _cache = new MemoryCache(options);
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"> 缓存Key </param>
        /// <param name="value"> 缓存Value </param>
        /// <returns> </returns>
        public bool Add<T>(string key, T value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            _cache.Set(key, value);
            return Exists(key);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"> 缓存Key </param>
        /// <param name="value"> 缓存Value </param>
        /// <param name="expiresSliding"> 滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间） </param>
        /// <param name="expiressAbsoulte"> 绝对过期时长 </param>
        /// <returns> </returns>
        public bool Add<T>(string key, T value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            _cache.Set(key, value,
                new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(expiresSliding)
                    .SetAbsoluteExpiration(expiressAbsoulte)
            );

            return Exists(key);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"> 缓存Key </param>
        /// <param name="value"> 缓存Value </param>
        /// <param name="expiresIn"> 缓存时长 </param>
        /// <param name="isSliding"> 是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间） </param>
        /// <returns> </returns>
        public bool Add<T>(string key, T value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (isSliding)
                _cache.Set(key, value,
                    new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(expiresIn)
                );
            else
                _cache.Set(key, value,
                    new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(expiresIn)
                );

            return Exists(key);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"> 缓存Key </param>
        /// <param name="value"> 缓存Value </param>
        /// <param name="expires"> 缓存时长 </param>
        /// <returns> </returns>
        /// <exception cref="ArgumentNullException"> </exception>
        public bool Add<T>(string key, T value, TimeSpan expires)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            _cache.Set(key, value,
                new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(expires));

            return Exists(key);
        }

        public bool Add<T>(string key, T value, bool isOverride)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            _cache.Set(key, value);
            return Exists(key);
        }

        /// <summary>
        /// 验证缓存项是否存在
        /// </summary>
        /// <param name="key"> 缓存Key </param>
        /// <returns> </returns>
        public bool Exists(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            return _cache.TryGetValue(key, out _);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"> 缓存Key </param>
        /// <returns> </returns>
        public T Get<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            return (T)_cache.Get(key);
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"> 缓存Key </param>
        /// <returns> </returns>
        public object Get(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            return _cache.Get(key);
        }
        /// <summary>
        /// 获取缓存集合
        /// </summary>
        /// <param name="keys"> 缓存Key集合 </param>
        /// <returns> </returns>
        public IDictionary<string, object> GetAll(IEnumerable<string> keys)
        {
            if (keys == null) throw new ArgumentNullException(nameof(keys));

            var dict = new Dictionary<string, object>();

            keys.ToList().ForEach(item => dict.Add(item, _cache.Get(item)));

            return dict;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"> 缓存Key </param>
        /// <returns> </returns>
        public string GetString(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            return _cache.Get(key)?.ToString();
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"> 缓存Key </param>
        /// <returns> </returns>
        public bool Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            _cache.Remove(key);

            return !Exists(key);
        }

        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <param name="keys"> 缓存Key集合 </param>
        /// <returns> </returns>
        public void RemoveAll(IEnumerable<string> keys)
        {
            if (keys == null) throw new ArgumentNullException(nameof(keys));

            keys.ToList().ForEach(item => _cache.Remove(item));
        }

        /// <summary>
        /// 修改缓存
        /// </summary>
        /// <param name="key"> 缓存Key </param>
        /// <param name="value"> 新的缓存Value </param>
        /// <returns> </returns>
        public bool Replace<T>(string key, T value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (Exists(key))
                if (!Remove(key))
                    return false;

            return Add(key, value);
        }

        /// <summary>
        /// 修改缓存
        /// </summary>
        /// <param name="key"> 缓存Key </param>
        /// <param name="value"> 新的缓存Value </param>
        /// <param name="timeSpan"> </param>
        /// <returns> </returns>
        public bool Replace<T>(string key, T value, TimeSpan timeSpan)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (Exists(key))
                if (!Remove(key))
                    return false;

            return Add(key, value, timeSpan);
        }

        /// <summary>
        /// 修改缓存
        /// </summary>
        /// <param name="key"> 缓存Key </param>
        /// <param name="value"> 新的缓存Value </param>
        /// <param name="expiresSliding"> 滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间） </param>
        /// <param name="expiressAbsoulte"> 绝对过期时长 </param>
        /// <returns> </returns>
        public bool Replace<T>(string key, T value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (Exists(key))
                if (!Remove(key))
                    return false;

            return Add(key, value, expiresSliding, expiressAbsoulte);
        }

        /// <summary>
        /// 修改缓存
        /// </summary>
        /// <param name="key"> 缓存Key </param>
        /// <param name="value"> 新的缓存Value </param>
        /// <param name="expiresIn"> 缓存时长 </param>
        /// <param name="isSliding"> 是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间） </param>
        /// <returns> </returns>
        public bool Replace<T>(string key, T value, TimeSpan expiresIn, bool isSliding)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (Exists(key))
                if (!Remove(key))
                    return false;

            return Add(key, value, expiresIn, isSliding);
        }
    }
}