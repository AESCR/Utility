using System.Text;

namespace Common.Utility.Extensions.System
{
    public static class HttpRequestExtensions
    {
        #region Public Methods

        public static string GetAbsoluteUri(this Microsoft.AspNetCore.Http.HttpRequest request)
        {
            return new StringBuilder()
                .Append(request.Scheme)
                .Append("://")
                .Append(request.Host)
                .Append(request.PathBase)
                .Append(request.Path)
                .Append(request.QueryString)
                .ToString();
        }

        public static string GetHostUri(this Microsoft.AspNetCore.Http.HttpRequest request)
        {
            return new StringBuilder()
                .Append(request.Scheme)
                .Append("://")
                .Append(request.Host)
                .Append(request.PathBase)
                .ToString();
        }

        #endregion Public Methods
    }
}