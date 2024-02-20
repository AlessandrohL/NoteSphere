using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.ActionFilters
{
    public class RemoveAuthorizationHeader : ActionFilterAttribute
    {
        private const string AuthorizationHeader = "Authorization";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Headers.ContainsKey(AuthorizationHeader))
            {
                context.HttpContext.Request.Headers.Remove(AuthorizationHeader);
            }

            base.OnActionExecuting(context);
        }

    }
}
