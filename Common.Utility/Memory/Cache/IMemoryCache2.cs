using System;
using System.Collections.Generic;
using Common.Utility.Autofac;

namespace Common.Utility.Memory.Cache
{
    public interface IMemoryCache2 : IMemoryCache, ISingletonDependency
    {
        bool Add<T>(string key, T value, TimeSpan expiresIn, bool isSliding = false);

        bool Add<T>(string key, T value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte);

        object Get(string key);
        IDictionary<string, object> GetAll(IEnumerable<string> keys);

        void RemoveAll(IEnumerable<string> keys);

        bool Replace<T>(string key, T value, TimeSpan expiresIn, bool isSliding);

        bool Replace<T>(string key, T value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte);
    }
}