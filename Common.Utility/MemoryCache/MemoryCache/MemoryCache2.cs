using System;
using System.Collections.Generic;
using System.Linq;
using Common.Utility.Memory.MemoryCache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Utility.MemoryCache
{

    public class MemoryCache2 : IMemoryCache2
    {
        #region Private Fields

        /// <summary>
        ///     MemoryCache 缓存
        /// </summary>
        private Microsoft.Extensions.Caching.Memory.IMemoryCache _cache;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        ///     构造器注入 IMemoryCache
        /// </summary>
        /// <param name="cache"></param>
        public MemoryCache2(Microsoft.Extensions.Caching.Memory.IMemoryCache cache)
        {
            _cache = cache;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <returns></returns>
        public bool Add(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            _cache.Set(key, value);
            return Exists(key);
        }

        /// <summary>
        ///     添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <returns></returns>
        public bool Add(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
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
        ///     添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresIn">缓存时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <returns></returns>
        public bool Add(string key, object value, TimeSpan expiresIn, bool isSliding = false)
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
        ///  添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expires">缓存时长</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool Add(string key, object value, TimeSpan expires)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            _cache.Set(key, value,
                new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(expires));

            return Exists(key);
        }

        public bool Add(string key, object value, bool isOverride = false)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            _cache.Set(key, value);
            return Exists(key);
        }

        /// <summary>
        ///     验证缓存项是否存在
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            return _cache.TryGetValue(key, out _);
        }

        /// <summary>
        ///     获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            return _cache.Get(key) as T;
        }

        /// <summary>
        ///     获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public object Get(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            return _cache.Get(key);
        }

        /// <summary>
        ///     获取缓存集合
        /// </summary>
        /// <param name="keys">缓存Key集合</param>
        /// <returns></returns>
        public IDictionary<string, object> GetAll(IEnumerable<string> keys)
        {
            if (keys == null) throw new ArgumentNullException(nameof(keys));

            var dict = new Dictionary<string, object>();

            keys.ToList().ForEach(item => dict.Add(item, _cache.Get(item)));

            return dict;
        }

        /// <summary>
        ///     删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            _cache.Remove(key);

            return !Exists(key);
        }

        /// <summary>
        ///     批量删除缓存
        /// </summary>
        /// <param name="keys">缓存Key集合</param>
        /// <returns></returns>
        public void RemoveAll(IEnumerable<string> keys)
        {
            if (keys == null) throw new ArgumentNullException(nameof(keys));

            keys.ToList().ForEach(item => _cache.Remove(item));
        }

        /// <summary>
        ///     修改缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <returns></returns>
        public bool Replace(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (Exists(key))
                if (!Remove(key))
                    return false;

            return Add(key, value);
        }
        /// <summary>
        ///     修改缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <returns></returns>
        public bool Replace(string key, object value, TimeSpan timeSpan)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (Exists(key))
                if (!Remove(key))
                    return false;

            return Add(key, value,timeSpan);
        }
        /// <summary>
        ///     修改缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <returns></returns>
        public bool Replace(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (Exists(key))
                if (!Remove(key))
                    return false;

            return Add(key, value, expiresSliding, expiressAbsoulte);
        }

        /// <summary>
        ///     修改缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <param name="expiresIn">缓存时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <returns></returns>
        public bool Replace(string key, object value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (Exists(key))
                if (!Remove(key))
                    return false;

            return Add(key, value, expiresIn, isSliding);
        }

        #endregion Public Methods
    }
}