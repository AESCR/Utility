using Common.Utility.Autofac;
using Common.Utility.Memory.Model;
using CSRedis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Common.Utility.Memory.Redis
{
    public class RedisCache : IRedisCache
    {
        private readonly RedisClient _redisClient;

        public RedisCache(Action<MemoryOptions> optionAction=null)
        {
            var options = new MemoryOptions
            {
                UseRedis = false,
                Host = "127.0.0.1",
                DbIndex = 0,
                Password = "",
                Port = 6379,
                ReconnectAttempts = 3,
                ReconnectWait = 200
            };
            optionAction?.Invoke(options);
            _redisClient = new RedisClient(options.Host, options.Port);
            if (!string.IsNullOrWhiteSpace(options.Password))
            {
                _redisClient.Connected += (s, e) =>
                {
                    _redisClient.Auth(options.Password);
                    _redisClient.Select(options.DbIndex);
                };
            }
            _redisClient.ReconnectAttempts = options.ReconnectAttempts;//失败后重试3次
            _redisClient.ReconnectWait = options.ReconnectWait;//在抛出异常之前，连接将在200ms之间重试3次
            _redisClient.Connect(options.Timeout);
        }

        ~RedisCache()
        {
            Dispose(false);
        }

        public bool IsConnect => _redisClient.IsConnected;

        /// <summary>
        /// 服务是否启动
        /// </summary>
        /// <returns> </returns>
        public bool Launch => _redisClient.Ping().ToLower() == "ok";

        private T DeserializeObject<T>(string value)
        {
            return value != null ? JsonConvert.DeserializeObject<T>(value) : default;
        }

        private string[] GetString<T>(List<T> list)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            int len = list.Count;
            string[] str = new string[len];
            for (int i = 0; i < len; i++)
            {
                var obj = list[i];
                str[i] = SerializeObject(obj);
            }

            return str;
        }

        private string SerializeObject(object value)
        {
            return value != null ? JsonConvert.SerializeObject(value) : String.Empty;
        }

        private bool stringOk(string val)
        {
            if (val == null)
            {
                return false;
            }
            return val.ToLower() == "ok";
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_redisClient.IsConnected)
                {
                    _redisClient.Quit();
                }
                _redisClient?.Dispose();
            }
        }

        /// <summary>
        /// 添加缓存 字符串
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="val"> </param>
        /// <param name="timeSpan"> </param>
        /// <param name="isOverride"> 是否覆盖 </param>
        /// <returns> </returns>
        public bool Add<T>(string key, T val, TimeSpan timeSpan, bool isOverride)
        {
            if (string.IsNullOrWhiteSpace(key)) return false;
            if (val == null) return false;
            var str = SerializeObject(val);
            if (isOverride)
            {
                Del(key);
            }
            var result = _redisClient.Set(key, str, timeSpan, RedisExistence.Nx);
            return stringOk(result);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="val"> </param>
        /// <returns> </returns>
        public bool Add<T>(string key, T val)
        {
            if (string.IsNullOrWhiteSpace(key)) return false;
            if (val == null) return false;
            var str = SerializeObject(val);
            return _redisClient.SetNx(key, str);
        }

        /// <summary>
        /// 添加一个字符串
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="val"> </param>
        /// <param name="isOverride"> </param>
        /// <returns> </returns>
        public bool Add<T>(string key, T val, bool isOverride)
        {
            if (string.IsNullOrWhiteSpace(key)) return false;
            if (val == null) return false;
            var str = SerializeObject(val);
            var redisExistence = RedisExistence.Nx;
            if (isOverride)
            {
                Del(key);
            }
            var result = _redisClient.Set(key, str, null, redisExistence);
            return stringOk(result);
        }

        public bool Add<T>(string key, T value, TimeSpan expires)
        {
            return Add(key, value, expires, false);
        }

        /// <summary>
        /// 设置一个Hash
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="dic"> </param>
        /// <param name="timeSpan"> </param>
        /// <returns> </returns>
        public bool AddHash<T>(string key, Dictionary<string, T> dic, TimeSpan timeSpan)
        {
            if (string.IsNullOrWhiteSpace(key)) return false;
            if (dic == null) return false;
            Dictionary<string, object> kJson = new Dictionary<string, object>();
            foreach (var oKey in dic.Keys)
            {
                var obj = dic[oKey];
                kJson.Add(oKey, SerializeObject(obj));
            }
            var result = _redisClient.HMSet(key, kJson);
            _redisClient.ExpireAsync(key, timeSpan);
            return stringOk(result);
        }

        public bool AddHash<T>(string key, Dictionary<string, T> dic)
        {
            if (string.IsNullOrWhiteSpace(key)) return false;
            if (dic == null) return false;
            Dictionary<string, object> kJson = new Dictionary<string, object>();
            foreach (var oKey in dic.Keys)
            {
                var obj = dic[oKey];
                kJson.Add(oKey, SerializeObject(obj));
            }
            var result = _redisClient.HMSet(key, kJson);
            return stringOk(result);
        }

        public bool AddHash<T>(string key, string field, T t)
        {
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(field)) return false;
            var json = SerializeObject(t);
            var result = _redisClient.HSet(key, field, json);
            return result;
        }

        /// <summary>
        /// 添加List
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="list"> </param>
        /// <returns> </returns>
        public long AddList<T>(string key, List<T> list)
        {
            if (string.IsNullOrWhiteSpace(key)) return 0;
            if (list == null) return 0;
            var str = GetString(list);
            var lg = _redisClient.RPush(key, str);
            return lg;
        }

        /// <summary>
        /// 添加List
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="val"> </param>
        /// <returns> </returns>
        public long AddList<T>(string key, T val)
        {
            if (string.IsNullOrWhiteSpace(key)) return 0;
            if (val == null) return 0;
            var str = SerializeObject(val);
            var lg = _redisClient.RPush(key, str);
            return lg;
        }

        /// <summary>
        /// 添加List
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="val"> </param>
        /// <param name="timeSpan"> </param>
        /// <returns> </returns>
        public long AddList<T>(string key, T val, TimeSpan timeSpan)
        {
            var result = AddList(key, val);
            _redisClient.ExpireAsync(key, timeSpan);
            return result;
        }

        /// <summary>
        /// 添加List
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="list"> </param>
        /// <param name="timeSpan"> </param>
        /// <returns> </returns>
        public long AddList<T>(string key, List<T> list, TimeSpan timeSpan)
        {
            if (string.IsNullOrWhiteSpace(key)) return 0;
            if (list == null) return 0;
            var lg = AddList(key, list);
            _redisClient.ExpireAsync(key, timeSpan);
            return lg;
        }

        /// <summary>
        /// 添加集合
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="obj"> </param>
        /// <returns> </returns>
        public long AddSet<T>(string key, T obj)
        {
            if (string.IsNullOrWhiteSpace(key)) return 0;
            var str = SerializeObject(obj);
            return _redisClient.SAdd(key, str);
        }

        /// <summary>
        /// 添加集合
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="list"> 集合列表 </param>
        /// <returns> </returns>
        public long AddSet<T>(string key, List<T> list)
        {
            if (string.IsNullOrWhiteSpace(key)) return 0;
            if (list == null) return 0;
            var str = GetString(list);
            return _redisClient.SAdd(key, str);
        }

        /// <summary>
        /// 添加集合
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="list"> </param>
        /// <param name="timeSpan"> </param>
        /// <returns> </returns>
        public long AddSet<T>(string key, List<T> list, TimeSpan timeSpan)
        {
            var result = AddSet(key, list);
            _redisClient.ExpireAsync(key, timeSpan);
            return result;
        }

        /// <summary>
        /// 有序集合(sorted set)
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="score"> </param>
        /// <param name="val"> </param>
        /// <returns> </returns>
        public long AddSortedSet(string key, double score, string val)
        {
            if (string.IsNullOrWhiteSpace(key)) return 0;
            Tuple<double, string> obj = new Tuple<double, string>(score, val);
            var lg = _redisClient.ZAdd(key, obj);
            return lg;
        }

        /// <summary>
        /// 有序集合(sorted set)
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="score"> </param>
        /// <param name="list"> </param>
        /// <returns> </returns>
        public long AddSortedSet<T>(string key, double[] score, List<T> list)
        {
            if (string.IsNullOrWhiteSpace(key)) return 0;
            Tuple<double, string>[] obj = new Tuple<double, string>[score.Length];
            string[] str = GetString(list);
            for (int i = 0; i < score.Length; i++)
            {
                Tuple<double, string> temp = new Tuple<double, string>(score[i], str[i]);
                obj[i] = temp;
            }
            var lg = _redisClient.ZAdd(key, obj);
            return lg;
        }

        /// <summary>
        /// 有序集合(sorted set)
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="score"> </param>
        /// <param name="val"> </param>
        /// <param name="timeSpan"> </param>
        /// <returns> </returns>
        public long AddSortedSet<T>(string key, double score, T val, TimeSpan timeSpan)
        {
            var lg = AddSortedSet(key, new double[] { score }, new List<object>() { val });
            _redisClient.ExpireAsync(key, timeSpan);
            return lg;
        }

        /// <summary>
        /// 关闭客户端
        /// </summary>
        /// <returns> </returns>
        public bool Close()
        {
            _redisClient?.Quit();
            _redisClient?.ClientKill();
            _redisClient?.Dispose();
            return true;
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"> </param>
        /// <returns> </returns>
        public bool Del(string key)
        {
            return _redisClient.Del(key) > 0;
        }

        /// <summary>
        /// 移除List中的某个值
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="val"> </param>
        /// <returns> </returns>
        public long DelList<T>(string key, T val)
        {
            string str = SerializeObject(val);
            return _redisClient.LRem(key, 0, str);
        }

        /// <summary>
        /// 移除集合中多个成员
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="members"> </param>
        /// <returns> </returns>
        public long DelSet<T>(string key, List<T> members)
        {
            string[] str = GetString(members);
            return _redisClient.SRem(key, str);
        }

        /// <summary>
        /// 移除集合中一个
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="members"> </param>
        /// <returns> </returns>
        public long DelSet<T>(string key, T members)
        {
            string str = SerializeObject(members);
            return _redisClient.SRem(key, str);
        }

        /// <summary>
        /// 移除有序集合中的一个或多个成员
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="members"> </param>
        /// <returns> </returns>
        public long DelSortedSet<T>(string key, List<T> members)
        {
            string[] str = GetString(members);
            return _redisClient.ZRem(key, str);
        }

        /// <summary>
        /// 移除有序集合中的一个
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="members"> </param>
        /// <returns> </returns>
        public long DelSortedSet<T>(string key, T members)
        {
            return DelSortedSet(key, new List<T>() { members });
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key"> </param>
        /// <returns> </returns>
        public bool Exists(string key)
        {
            return _redisClient.Exists(key);
        }

        public bool Expire(string key, TimeSpan timeSpan)
        {
            return _redisClient.Expire(key, timeSpan);
        }

        /// <summary>
        /// 获取实体 支持String Hash
        /// </summary>
        /// <param name="key"> </param>
        /// <typeparam name="T"> </typeparam>
        /// <returns> </returns>
        public T Get<T>(string key)
        {
            var json = GetString(key);
            return DeserializeObject<T>(json);
        }

        /// <summary>
        /// 查询Hash 表
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="field"> </param>
        /// <returns> </returns>
        public string Get(string key, string field)
        {
            return _redisClient.HGet(key, field);
        }

        /// <summary>
        /// 查询Hash 表
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="field"> </param>
        /// <returns> </returns>
        public string[] Get(string key, string[] field)
        {
            return _redisClient.HMGet(key, field);
        }

        public RedisClient GetClient()
        {
            return _redisClient;
        }

        /// <summary>
        /// 查询数据集合 支持Set List SortedSet类型 查询
        /// </summary>
        /// <param name="key"> key </param>
        /// <returns> </returns>
        /// <exception cref="RedisException"> </exception>
        public string[] GetCollection(string key)
        {
            var type = GetType(key);
            if (type == RedisVaule.Set)
            {
                return GetRange(key);
            }
            else if (type == RedisVaule.List)
            {
                return GetLRange(key);
            }
            else if (type == RedisVaule.SortedSet)
            {
                return GetZRange(key);
            }
            throw new RedisException($"未查询到key：{key}");
        }

        /// <summary>
        /// 查询List 类型集合
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="start"> </param>
        /// <param name="len"> </param>
        /// <returns> </returns>
        public string[] GetLRange(string key, long start = 0, long len = -1)
        {
            if (len < 0)
            {
                len = _redisClient.LLen(key) - 1;
            }
            return _redisClient.LRange(key, start, len);
        }

        /// <summary>
        /// 查询 Set 集合
        /// </summary>
        /// <param name="key"> </param>
        /// <returns> </returns>
        public string[] GetRange(string key)
        {
            return _redisClient.SMembers(key);
        }

        /// <summary>
        /// 查询字符串
        /// </summary>
        /// <param name="key"> </param>
        /// <returns> </returns>
        public string GetString(string key)
        {
            return _redisClient.Get(key);
        }

        public RedisVaule GetType(string key)
        {
            var type = _redisClient.Type(key);
            return type switch
            {
                "hash" => RedisVaule.Hash,
                "string" => RedisVaule.String,
                "list" => RedisVaule.List,
                "set" => RedisVaule.Set,
                "zset" => RedisVaule.SortedSet,
                _ => RedisVaule.None
            };
        }

        /// <summary>
        /// 查询 SortedSet 有序集合
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="start"> </param>
        /// <param name="len"> </param>
        /// <returns> </returns>
        public string[] GetZRange(string key, long start = 0, long len = -1)
        {
            if (len < 0)
            {
                len = _redisClient.ZCard(key) - 1;
            }
            return _redisClient.ZRange(key, start, len, true);
        }

        /// <summary>
        /// 发布消息到频道
        /// </summary>
        /// <param name="name"> 频道名称 </param>
        /// <param name="message"> 消息 </param>
        /// <returns> </returns>
        public long Publish(string name, string message)
        {
            return _redisClient.Publish(name, message);
        }

        /// <summary>
        /// 退订所有给定模式的频道。
        /// </summary>
        public void PUnsubscribe(params string[] channelPatterns)
        {
            _redisClient.PUnsubscribe(channelPatterns);
        }

        public bool Remove(string key)
        {
            return Del(key);
        }

        /// <summary>
        /// 修改缓存 string 类型
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="obj"> </param>
        /// <param name="timeSpan"> </param>
        /// <returns> </returns>
        public bool Replace<T>(string key, T obj, TimeSpan timeSpan)
        {
            var str = SerializeObject(obj);
            var result = _redisClient.Set(key, str, timeSpan, RedisExistence.Xx);
            return stringOk(result);
        }

        /// <summary>
        /// 修改缓存 string 类型
        /// </summary>
        /// <param name="key"> </param>
        /// <param name="obj"> </param>
        /// <param name="timeSpan"> </param>
        /// <returns> </returns>
        public bool Replace<T>(string key, T obj)
        {
            var str = SerializeObject(obj);
            var result = _redisClient.Set(key, str, null, RedisExistence.Xx);
            return stringOk(result);
        }

        /// <summary>
        /// 订阅频道
        /// </summary>
        /// <param name="name"> 频道名称 </param>
        public void Subscribe(params string[] name)
        {
            _redisClient.Subscribe(name);
        }

        /// <summary>
        /// 切换数据库
        /// </summary>
        /// <param name="index"> </param>
        /// <returns> </returns>
        public bool SwitchDb(int index)
        {
            return _redisClient.Select(index) == "Ok";
        }

        /// <summary>
        /// 指退订给定的频道。
        /// </summary>
        /// <param name="name"> 频道名称 </param>
        public void Unsubscribe(params string[] name)
        {
            _redisClient.Unsubscribe(name);
        }
    }
}