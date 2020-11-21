using System;
using Newtonsoft.Json;

namespace Common.Utility.Utils
{
    public class ConvertUtils
    {
        public static string SerializeObject(object value,SerializeType type=SerializeType.Json)
        {
            switch (type)
            {
                
                case SerializeType.Xml:
                    break;
                case SerializeType.Json:
                    return JsonConvert.SerializeObject(value);
                    break;
                default:
                    break;
            }
            throw new Exception("序列化失败!");
        }

        public static T DeserializeObject<T>(string val,SerializeType type=SerializeType.Json)
        {
            switch (type)
            {
                
                case SerializeType.Xml:
                    break;
                case SerializeType.Json:
                    return JsonConvert.DeserializeObject<T>(val);
                    break;
                default:
                    break;
            }
            throw new Exception("反序列化失败!");
          
        }
    }

    public enum SerializeType
    {
        Xml,
        Json
    }
}