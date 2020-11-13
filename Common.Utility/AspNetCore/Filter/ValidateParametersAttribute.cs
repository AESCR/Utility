using System.Linq;
using System.Net;
using System.Text;
using Common.Utility.HttpResponse;
using Common.Utility.Json;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Common.Utility.AspNetCore.Filter
{
    public class ValidateParametersAttribute : ActionFilterAttribute
    {
        #region Public Methods

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid) return;
            var result = context.ModelState.Keys
                .SelectMany(key =>
                    context.ModelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                .ToList();
            context.HttpContext.Response.StatusCode = (int) HttpStatusCode.OK;
            ResponseResult responseResult = new ResponseResult {Data = result, Type = ResponseEnum.Validation};
            context.HttpContext.Response.Body.Write(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(responseResult)));
        }

        #endregion Public Methods
    }
    public class ValidationError
    {
        #region Public Constructors

        public ValidationError(string field, string message)
        {
            Field = field != string.Empty ? field : null;
            Message = message;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Field { get; set; }
        public string Message { get; set; }

        #endregion Public Properties
    }
}