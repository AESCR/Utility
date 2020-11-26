using CSRedis;
using System;
using System.Collections.Generic;
using Common.Utility.Autofac;

namespace Common.Utility.Memory.Redis
{
    public interface IRedisCache : IMemoryCache, IScopedDependency, IRelease
    {
        bool Launch { get; }

        bool Add<T>(string key, T val, TimeSpan timeSpan, bool isOverride);

        bool AddHash<T>(string key, Dictionary<string, T> dic, TimeSpan timeSpan);

        bool AddHash<T>(string key, string field, T t);

        bool AddHash<T>(string key, Dictionary<string, T> dic);

        long AddList<T>(string key, List<T> list);

        long AddList<T>(string key, T val, TimeSpan timeSpan);

        long AddList<T>(string key, T val);

        long AddList<T>(string key, List<T> list, TimeSpan timeSpan);

        long AddSet<T>(string key, List<T> list);

        long AddSet<T>(string key, List<T> list, TimeSpan timeSpan);

        long AddSet<T>(string key, T obj);

        long AddSortedSet(string key, double score, string val);

        long AddSortedSet<T>(string key, double score, T val, TimeSpan timeSpan);

        long AddSortedSet<T>(string key, double[] score, List<T> list);

        bool Close();

        bool Del(string key);

        long DelList<T>(string key, T val);

        long DelSet<T>(string key, List<T> members);

        long DelSet<T>(string key, T members);

        long DelSortedSet<T>(string key, List<T> members);

        long DelSortedSet<T>(string key, T members);

        bool Expire(string key, TimeSpan timeSpan);

        string Get(string key, string field);

        new T Get<T>(string key);

        string[] Get(string key, string[] field);

        RedisClient GetClient();

        string[] GetCollection(string key);

        string[] GetLRange(string key, long start = 0, long len = -1);

        string[] GetRange(string key);

        RedisVaule GetType(string key);

        string[] GetZRange(string key, long start = 0, long len = -1);

        long Publish(string name, string message);

        void PUnsubscribe(params string[] channelPatterns);

        void Subscribe(params string[] name);

        bool SwitchDb(int index);

        void Unsubscribe(params string[] name);
    }
}