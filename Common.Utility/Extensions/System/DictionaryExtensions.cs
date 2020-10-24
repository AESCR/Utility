using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Common.Utility.SystemExtensions
{
    public static class DictionaryExtensions
    {
        #region Public Methods

        public static T ChangeType<T>(this Dictionary<string, object> dic, string key, bool isNullDefault = true) where T : class
        {
            if (!dic.ContainsKey(key))
            {
                return null;
            }
            object value = dic.GetValue(key);
            T result = JsonConvert.DeserializeObject<T>(value == null ? null : value.ToString());
            return result;
        }

        public static bool GetBoolean(this Dictionary<string, string> dic, string key, bool isNullDefault = true, string defaultValue = "false")
        {
            string value = dic.GetString(key, isNullDefault, defaultValue);
            return value.ToLower() == "true" || value == "1";
        }

        public static bool GetBoolean(this Dictionary<string, object> dic, string key, bool isNullDefault = true, string defaultValue = "false")
        {
            string value = dic.GetString(key, isNullDefault, defaultValue);
            return value.ToLower() == "true" || value == "1";
        }

        public static decimal GetDecimal(this Dictionary<string, object> dic, string key, bool isNullDefault = true, decimal defaultValue = 0)
        {
            if (dic != null && dic.ContainsKey(key))
            {
                return Convert.ToDecimal(dic[key]);
            }
            else if (isNullDefault)
                return defaultValue;
            else
                throw new Exception($"数据'{key}'丢失！！");
        }

        public static decimal GetDecimal(this Dictionary<string, string> dic, string key, bool isNullDefault = true, decimal defaultValue = 0)
        {
            if (dic != null && dic.ContainsKey(key))
            {
                return Convert.ToDecimal(dic[key]);
            }
            else if (isNullDefault)
            {
                return defaultValue;
            }
            else
            {
                throw new Exception($"数据'{key}'丢失！！");
            }
        }

        public static double GetDouble(this Dictionary<string, object> dic, string key, bool isNullDefault = true, double defaultValue = 0)
        {
            if (dic != null && dic.ContainsKey(key))
            {
                return Convert.ToDouble(dic[key]);
            }
            else if (isNullDefault)
                return defaultValue;
            else
                throw new Exception($"数据'{key}'丢失！！");
        }

        public static double GetDouble(this Dictionary<string, string> dic, string key, bool isNullDefault = true, double defaultValue = 0)
        {
            if (dic != null && dic.ContainsKey(key))
            {
                return Convert.ToDouble(dic[key]);
            }
            else if (isNullDefault)
                return defaultValue;
            else
                throw new Exception($"数据'{key}'丢失！！");
        }

        public static float GetFloat(this Dictionary<string, string> dic, string key, bool isNullDefault = true, double defaultValue = 0)
        {
            return (float)dic.GetDouble(key, isNullDefault, defaultValue);
        }

        public static float GetFloat(this Dictionary<string, object> dic, string key, bool isNullDefault = true, double defaultValue = 0)
        {
            return (float)dic.GetDouble(key, isNullDefault, defaultValue);
        }

        public static int GetInt32(this Dictionary<string, object> dic, string key, bool isNullDefault = true, int defaultValue = 0)
        {
            if (dic != null && dic.ContainsKey(key))
                return Convert.ToInt32(dic[key]);
            else if (isNullDefault)
                return defaultValue;
            else
                throw new Exception($"数据'{key}'丢失！！");
        }

        public static int GetInt32(this Dictionary<string, string> dic, string key, bool isNullDefault = true, int defaultValue = 0)
        {
            if (dic != null && dic.ContainsKey(key))
                return Convert.ToInt32(dic[key] ?? "" + defaultValue);
            else if (isNullDefault)
                return defaultValue;
            else
                throw new Exception($"数据'{key}'丢失！！");
        }

        public static T GetKey<T>(this Dictionary<string, T> dic, string key, T defaultValue = default(T))
        {
            if (dic.ContainsKey(key))
            {
                return dic[key];
            }
            else
            {
                return defaultValue;
            }
        }

        public static string GetString(this Dictionary<string, string> dic, string key, bool isNullDefault = true, string Default = "")
        {
            if (dic != null && dic.ContainsKey(key))
            {
                return dic[key];
            }
            else if (isNullDefault)
            {
                return Default;
            }
            else
            {
                throw new Exception($"数据'{key}'丢失！！");
            }
        }

        public static string GetString(this Dictionary<string, object> dic, string key, bool isNullDefault = true, string defaultValue = "")
        {
            if (dic != null && dic.ContainsKey(key))
            {
                object obj = dic[key];
                if (obj == null)
                {
                    return "";
                }
                return Convert.ToString(dic[key]);
            }
            else if (isNullDefault)
            {
                return defaultValue;
            }
            else
            {
                throw new Exception($"数据'{key}'丢失！！");
            }
        }

        public static object GetValue(this Dictionary<string, object> dic, string key, bool isNullDefault = true, string Default = "")
        {
            if (dic != null && dic.ContainsKey(key))
            {
                return dic[key];
            }
            else if (isNullDefault)
            {
                return Default;
            }
            else
            {
                throw new Exception($"数据'{key}'丢失！！");
            }
        }

        public static void SetKey<T>(this Dictionary<string, T> dic, string key, T value)
        {
            if (dic.ContainsKey(key))
            {
                dic[key] = value;
            }
            else
            {
                dic.Add(key, value);
            }
        }

        #endregion Public Methods
    }
}