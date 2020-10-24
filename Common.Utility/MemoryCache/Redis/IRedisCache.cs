using CSRedis;
using System;
using System.Collections.Generic;

namespace Common.Utility.MemoryCache.Redis
{
    public interface IRedisCache
    {
        bool Launch { get; }

        bool Add(string key, object val);

        bool Add(string key, object val, bool isOverride = false);

        bool Add(string key, object val, TimeSpan expires);

        bool Add(string key, object val, TimeSpan timeSpan, bool isOverride = false);

        bool AddHash(string key, Dictionary<string, object> dic, TimeSpan timeSpan);

        bool AddHash(string key, Dictionary<string, string> dic);

        long AddList(string key, List<object> list);

        long AddList(string key, List<object> list, TimeSpan timeSpan);

        long AddSet(string key, List<object> list);

        long AddSet(string key, List<object> list, TimeSpan timeSpan);

        long AddSet(string key, object str);

        long AddSortedSet(string key, double score, object val, TimeSpan timeSpan);

        long AddSortedSet(string key, double score, string val);

        long AddSortedSet(string key, double[] score, List<object> list);

        bool Close();

        long DelSortedSet(string key, List<object> members);

        long DelSortedSet(string key, object members);

        bool Exists(string key);

        object Get(string key);

        T Get<T>(string key) where T : class;

        RedisClient GetClient();

        string[] GetCollection(string key);

        string[] GetLRange(string key, long start = 0, long len = -1);

        string[] GetRange(string key);

        RedisVaule GetType(string key);

        string[] GetZRange(string key, long start = 0, long len = -1);

        string HGet(string key, string field);

        long Publish(string name, string message);

        void PUnsubscribe(params string[] channelPatterns);

        bool Remove(string key);

        long RemoveList(string key, object val);

        long RemoveSet(string key, List<object> members);

        long RemoveSet(string key, object members);

        bool Replace(string key, object obj);

        bool Replace(string key, object obj, TimeSpan timeSpan);

        void Subscribe(params string[] name);

        bool SwitchDb(int index);

        void Unsubscribe(params string[] name);
    }
}