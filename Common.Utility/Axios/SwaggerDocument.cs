using System.Collections.Generic;
using System.Linq;

namespace Common.Utility.Axios
{
    public class SwaggerDocument
    {
        public SwaggerInfo Info { get; set; }
        public string OpenApi { get; set; }
        public Dictionary<string, SwaggerRequest> Paths { get; set; }
        public List<TagsItem> Tags { get; set; }

        public Dictionary<string, string> TagsDic
        {
            get
            {
                if (Tags != null)
                {
                    return Tags.ToDictionary(x => x.Name, x => x.Description);
                }
                return new Dictionary<string, string>();
            }
        }
    }

    public class SwaggerInfo
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public string Version { get; set; }
    }

    public class SwaggerRequest
    {
        public SwaggerRequestMethod Delete { get; set; }
        public SwaggerRequestMethod Get { get; set; }
        public SwaggerRequestMethod Head { get; set; }
        public SwaggerRequestMethod Post { get; set; }
        public SwaggerRequestMethod Put { get; set; }
    }

    public class SwaggerRequestBody
    {
        public object content { get; set; }
        public string Description { get; set; }
    }

    public class SwaggerRequestMethod
    {
        public List<SwaggerRequestParameters> Parameters { get; set; } = new List<SwaggerRequestParameters>();
        public SwaggerRequestBody RequestBody { get; set; }
        public string Summary { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }

    public class SwaggerRequestParameters
    {
        public string Description { get; set; }
        public string In { get; set; }
        public string Name { get; set; }
    }

    public class SwaggerSchema
    {
        public string Format { get; set; }
        public string Type { get; set; }
    }

    public class TagsItem
    {
        /// <summary>
        /// ¿ØÖÆÆ÷×¢ÊÍ
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ¿ØÖÆÆ÷Ãû³Æ
        /// </summary>
        public string Name { get; set; }
    }
}