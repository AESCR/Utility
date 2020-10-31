namespace Common.Utility.HttpResponse
{
    /// <summary>
    /// 0-500
    /// </summary>
    public enum SysCode
    {
        /// <summary>
        /// 拒绝IP访问
        /// </summary>
        IPAccessDenied
    }

    public static class ResponseUtils
    {
        #region Public Methods

        public static ResponseBody GetResponseByCode(long code)
        {
            ResponseBody response = new ResponseBody { Code = code };
            //Todo 通过Code查询出响应体
            return response;
        }

        #endregion Public Methods
    }

    public class ResponseBody
    {
        #region Public Properties

        /// <summary>
        /// 消息对应码
        /// </summary>
        public long Code { get; set; }

        /// <summary>
        /// 返回对象载体
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        #endregion Public Properties
    }
}