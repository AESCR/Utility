using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Common.Utility.Extensions.HttpClient
{
    public static class HttpResponseMessageEx
    {
        public static T ReadModel<T>(this HttpResponseMessage httpResponse, out bool succeed) where T : class
        {
            succeed = false;
            try
            {
                if (!httpResponse.IsSuccessStatusCode) return null;
                var taskStream = httpResponse.Content.ReadAsStreamAsync();
                taskStream.Wait();
                using var dataStream = taskStream.Result;
                var reader = new StreamReader(dataStream);
                var s = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                var t = JsonConvert.DeserializeObject<T>(s);
                succeed = true;
                return t;
            }
            catch
            {
                return null;
            }
        }

        public static T ReadModel<T>(this Task<HttpResponseMessage> httpResponseTask, out bool succeed) where T : class
        {
            HttpResponseMessage httpResponse;
            try
            {
                httpResponse = httpResponseTask.Result;
            }
            catch
            {
                succeed = false;
                return null;
            }
            return ReadModel<T>(httpResponse, out succeed);
        }

        public static string ReadString(this HttpResponseMessage httpResponse, out bool succeed)
        {
            succeed = false;
            var statusText = httpResponse.StatusCode.ToChsText();
            try
            {
                var taskStream = httpResponse.Content.ReadAsStreamAsync();
                taskStream.Wait();
                using var dataStream = taskStream.Result;
                var reader = new StreamReader(dataStream);
                var result = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                succeed = true;
                return result;
            }
            catch
            {
                succeed = false;
                return statusText;
            }
        }

        public static string ReadString(this Task<HttpResponseMessage> httpResponseTask, out bool succeed)
        {
            HttpResponseMessage httpResponse;
            try
            {
                httpResponse = httpResponseTask.Result;
            }
            catch
            {
                succeed = false;
                return String.Empty;
            }
            return ReadString(httpResponse, out succeed);
        }
    }
}