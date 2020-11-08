using System;
using System.Collections.Generic;

namespace Common.Utility.Memory.Cache
{
    public interface IMemoryCache2 : IMemoryCache
    {
        #region Public Methods

        bool Add<T>(string key, T value, TimeSpan expiresIn, bool isSliding = false);

        bool Add<T>(string key, T value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte);

        IDictionary<string, object> GetAll(IEnumerable<string> keys);

        void RemoveAll(IEnumerable<string> keys);

        bool Replace<T>(string key, T value, TimeSpan expiresIn, bool isSliding);

        bool Replace<T>(string key, T value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte);

        #endregion Public Methods
    }
}