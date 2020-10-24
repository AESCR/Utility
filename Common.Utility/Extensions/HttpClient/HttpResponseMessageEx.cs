using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common.Utility.Extensions.HttpClient
{
    public static class HttpResponseMessageEx
    {
        #region Public Methods

        public static T ReadModel<T>(this HttpResponseMessage httpResponse) where T : class
        {
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
                JsonSerializerOptions o = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var t = JsonSerializer.Deserialize<T>(s, o);
                return t;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static T ReadModel<T>(this Task<HttpResponseMessage> httpResponseTask) where T : class
        {
            try
            {
                var httpResponse = httpResponseTask.Result;
                if (!httpResponse.IsSuccessStatusCode) return null;
                var taskStream = httpResponse.Content.ReadAsStreamAsync();
                taskStream.Wait();
                using var dataStream = taskStream.Result;
                var reader = new StreamReader(dataStream);
                var s = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                JsonSerializerOptions o = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var t = JsonSerializer.Deserialize<T>(s, o);
                return t;
            }
            catch
            {
                return null;
            }
        }

        public static string ReadString(this HttpResponseMessage httpResponse)
        {
            try
            {
                var taskStream = httpResponse.Content.ReadAsStreamAsync();
                taskStream.Wait();
                using var dataStream = taskStream.Result;
                var reader = new StreamReader(dataStream);
                var result = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                return result;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string ReadString(this Task<HttpResponseMessage> httpResponseTask)
        {
            var httpResponse = httpResponseTask.Result;
            var taskStream = httpResponse.Content.ReadAsStreamAsync();
            taskStream.Wait();
            var dataStream = taskStream.Result;
            var reader = new StreamReader(dataStream);
            var result = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            return result;
        }

        #endregion Public Methods
    }
}