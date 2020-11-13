using System.ComponentModel;
using Common.Utility.SystemExtensions;

namespace Common.Utility.HttpResponse
{
    /// <summary>
    /// 相同结果
    /// </summary>
    public class ResponseResult
    {
        /// <summary>
        /// 响应类型
        /// </summary>
        public ResponseEnum Type { get; set; } = ResponseEnum.Ok;
        /// <summary>
        /// 响应数据
        /// </summary>
        public object Data{ get; set; }
        /// <summary>
        /// 请求数据成功
        /// </summary>
        public bool IsSucceed => Type == ResponseEnum.Ok;

        /// <summary>
        /// 响应描述
        /// </summary>
        public string ResponseDesc
        {
            get;
            set;
        }
        /// <summary>
        /// 响应标题
        /// </summary>

        public string Title => Type.ToDescription();
    }

    /// <summary>
    /// 响应类型
    /// </summary>
    public enum ResponseEnum
    {
        [Description("请求成功")]
        Ok,
        [Description("验证错误")]
        Validation,
    }
}