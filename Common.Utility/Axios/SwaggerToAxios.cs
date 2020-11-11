using Common.Utility.Extensions.HttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Common.Utility.Axios
{
    public class AxiosDocument
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 参数注释
        /// </summary>
        public Dictionary<string, string> Notes { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 参数
        /// </summary>
        public List<string> Parameters { get; set; } = new List<string>();

        /// <summary>
        /// 请求地址
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 方法注释
        /// </summary>
        public string Summary { get; set; }

        public string Tags { get; set; }

        /// <summary>
        /// 请求类型
        /// </summary>
        public string Type { get; set; }
    }

    public class SwaggerToAxios
    {
        private Dictionary<string, List<AxiosDocument>> dicResult;

        private string instance = @"
import axios from 'axios'
// 使用由库提供的配置的默认值来创建实例 此时超时配置的默认值是 `0`
var instance = axios.create();
// 覆写库的超时默认值 现在，在超时前，所有请求都会等待 2.5 秒
instance.defaults.timeout = 2500;
instance.interceptors.request.use(
    function (config) {
        // 在发送请求之前做些什么
        return config;
    },
    function (error) {
        // 对请求错误做些什么
        return Promise.reject(error);
    }
)
// 添加响应拦截器
instance.interceptors.response.use(
    function (response) {
        // 对响应数据做点什么
        return response;
    },
    function (error) {
        // 对响应错误做点什么
        let resp = error.response;
        return Promise.reject(error);
    }
)
export default instance;
";

        private string tempAxios = @"
import axios from '../instance.js'
const apiService = {
    $content$
}
export default apiService
";

        private string tempImport = @"
$import$
const api = {
$importContent$
}
export default api
";

        private string tempMethod = @"
/**
 * @method $methodName$
 * $@param$
 * @return {promise}
 * @desc  $desc$
 */
$method$($param$){
    $paramsContent$
    return axios.$type$('$url$', config);
},";

        // config.params = { '$key$': value }
        private SwaggerRequestMethod GetRequest(SwaggerRequest swaggerRequest, AxiosDocument axios)
        {
            if (swaggerRequest.Delete != null)
            {
                axios.Type = "delete";
                return swaggerRequest.Delete;
            }
            else if (swaggerRequest.Get != null)
            {
                axios.Type = "get";
                return swaggerRequest.Get;
            }
            else if (swaggerRequest.Head != null)
            {
                axios.Type = "head";
                return swaggerRequest.Head;
            }
            else if (swaggerRequest.Post != null)
            {
                axios.Type = "post";
                return swaggerRequest.Post;
            }
            else if (swaggerRequest.Put != null)
            {
                axios.Type = "put";
                return swaggerRequest.Put;
            }
            return null;
        }

        public SwaggerToAxios ReadSwagger(string httpPath)
        {
            using (
                HttpClient httpClient = new HttpClient())
            {
                List<AxiosDocument> axios = new List<AxiosDocument>();
                var result = httpClient.DoGet(httpPath).ReadModel<SwaggerDocument>(out _);
                foreach (KeyValuePair<string, SwaggerRequest> keyValuePair in result.Paths)
                {
                    AxiosDocument ad = new AxiosDocument();
                    ad.Path = keyValuePair.Key;
                    ad.Method = ad.Path.Substring(ad.Path.LastIndexOf("/", StringComparison.Ordinal) + 1);
                    var srm = GetRequest(keyValuePair.Value, ad);
                    ad.Summary = srm.Summary;
                    foreach (var swp in srm.Parameters)
                    {
                        ad.Notes.Add(swp.Name.ToLower(), swp.Description);
                        ad.Parameters.Add(swp.Name.ToLower());
                    }
                    ad.Tags = srm.Tags.FirstOrDefault()?.ToLower();

                    if (ad.Tags != null) ad.FilePath = $"./webapi/{ad.Tags.ToLower()}service.js";
                    axios.Add(ad);
                }

                dicResult = axios.GroupBy(x => x.Tags + "service").ToDictionary(x => x.Key, x => x.ToList());
                WriteAxios();
                return this;
            }
        }

        public Dictionary<string, byte[]> WriteAxios()
        {
            Dictionary<string, byte[]> fileBytes = new Dictionary<string, byte[]>();
            StringBuilder importFile = new StringBuilder();
            StringBuilder importName = new StringBuilder();
            foreach (string key in dicResult.Keys)
            {
                var axios = dicResult[key];
                StringBuilder sb = new StringBuilder();
                foreach (var axio in axios)
                {
                    string temp = tempMethod;

                    temp = temp.Replace("$desc$", axio.Summary);

                    temp = temp.Replace("$methodName$", axio.Method);
                    string param = "";
                    string paramsContent = "";
                    string paramdata = "";
                    //@param data 目标对象
                    foreach (string parameter in axio.Parameters)
                    {
                        param += parameter + ",";
                        paramsContent += $"{parameter}:{parameter},";
                    }
                    foreach (var k in axio.Notes.Keys)
                    {
                        paramdata += $"* @param {k} {axio.Notes[k]}" + Environment.NewLine;
                    }
                    temp = temp.Replace("$@param$", paramdata);

                    temp = temp.Replace("$url$", axio.Path);
                    temp = temp.Replace("$type$", axio.Type);

                    if (paramsContent.Length > 0)
                    {
                        var x = temp.Replace("$paramsContent$", $"let config=" + "{params:{" + paramsContent + "}}");
                        if (param.Length > 0)
                        {
                            x = x.Replace("$param$", param.Substring(0, param.Length - 1));
                        }
                        x = x.Replace("$method$", axio.Method + "byParams");
                        x = x + Environment.NewLine;
                        sb.AppendLine(x);
                    }
                    temp = temp.Replace("$param$", "config");
                    temp = temp.Replace("$paramsContent$", "");
                    temp = temp.Replace("$method$", axio.Method);
                    temp = temp + Environment.NewLine;
                    sb.AppendLine(temp);
                }

                string ta = tempAxios;
                ta = ta.Replace("$content$", sb.ToString());
                byte[] bytes = Encoding.UTF8.GetBytes(ta);
                importFile.AppendLine($"import {key} from './webapi/{key}.js'");
                importName.AppendLine($"{key},");
                fileBytes.Add($"webapi/{key}.js", bytes);
            }

            string import = tempImport;
            import = import.Replace("$import$", importFile.ToString());
            import = import.Replace("$importContent$", importName.ToString());
            fileBytes.Add("apis.js", Encoding.UTF8.GetBytes(import));
            fileBytes.Add("instance.js", Encoding.UTF8.GetBytes(instance));
            return fileBytes;
        }
    }
}