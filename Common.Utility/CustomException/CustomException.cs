using Common.Utility.HttpResponse;
using Common.Utility.SystemExtensions;

namespace Common.Utility.Exception
{
    /// <summary>
    /// 自定义异常
    /// </summary>
    public class CustomException: System.Exception
    {
        private readonly ResponseEnum _responseEnum;

        public override string Message => _responseEnum.ToDescription();

        public CustomException(ResponseEnum responseEnum) 
        {
            this._responseEnum = responseEnum;
        }
    }
}
