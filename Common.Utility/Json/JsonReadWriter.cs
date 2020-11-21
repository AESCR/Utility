using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Common.Utility.Json
{
    public  class JsonReadWriter
    {
        private string JsonStr { get; set; }=String.Empty;
        public static JsonReadWriter Load(string path)
        {
            var jsonReadWriter=new JsonReadWriter();
            if (!File.Exists(path)) return jsonReadWriter;
            using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            var sr = new StreamReader(fileStream, Encoding.UTF8);
            jsonReadWriter.JsonStr = sr.ReadToEnd();
            sr.Close();
            fileStream.Close();
            return jsonReadWriter;
        }
        public static JsonReadWriter Load<T>(T model) where T:class
        {
            var jsonReadWriter=new JsonReadWriter();
            if (model!=null)
            {
                jsonReadWriter.JsonStr = JsonConvert.SerializeObject(model);
            }
            return jsonReadWriter;
        }

        public static JsonReadWriter LoadString(string json)
        {
            var jsonReadWriter=new JsonReadWriter();
            jsonReadWriter.JsonStr = json;
            return jsonReadWriter;
        }

        public T Deserialize<T>()where T:class
        {
            return  JsonConvert.DeserializeObject<T>(JsonStr);
        } 
        /// <summary>
        /// 保存的路径默认根目录
        /// </summary>
        /// <param name="path"></param>
        public  bool Save(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }
            var fileName= Path.GetFileName(path)+ ".json";
            using var fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write,FileShare.ReadWrite);
            StreamWriter sw=new StreamWriter(fileName,false,Encoding.UTF8);
            sw.Write(JsonStr);
            return true;
        }
    }
}