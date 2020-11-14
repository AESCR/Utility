using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Common.Utility.AspNetCore.Filter
{
    /// <summary>
    ///     全局错误
    /// </summary>
    public class ExceptionsFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionsFilter> _logger;

        public ExceptionsFilter(ILogger<ExceptionsFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///     错误发生
        /// </summary>
        /// <param name="context"> </param>
        public void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled) //如果异常没处理
            {
                context.ExceptionHandled = true; //设置异常已被处理
                //判断请求是否来源ajax
                if (context.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest")
                {
                    if (context.Exception is CustomException.CustomException ce)
                    {
                        var msg = ce.Message;
                    }

                    context.Result = new BadRequestObjectResult("服务器端发生错误!");
                }
                else
                {
                    context.Result = new BadRequestObjectResult(context.Exception.Message);
                }

                _logger.LogError(context.Exception.Message);
            }
        }
    }
}