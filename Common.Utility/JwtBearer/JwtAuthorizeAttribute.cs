using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Common.Utility.JwtBearer
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private static bool HasAllowAnonymous(AuthorizationFilterContext context)
        {
            var filters = context.Filters;
            if (filters.OfType<IAllowAnonymousFilter>().Any()) return true;

            // When doing endpoint routing, MVC does not add AllowAnonymousFilters for
            // AllowAnonymousAttributes that were discovered on controllers and actions. To maintain
            // compat with 2.x, we'll check for the presence of IAllowAnonymous in endpoint metadata.
            var endpoint = context.HttpContext.GetEndpoint();
            return endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.Items["User"];
            if (HasAllowAnonymous(context) != false) return;
            if (user == null)
            {
                // not logged in
                context.Result = new UnauthorizedResult();
            }
            else //检查用户是否存在URL访问权限
            {
                context.Result =
                    new BadRequestObjectResult($"用户，不具备访问的权限。");
            }
        }
    }
}