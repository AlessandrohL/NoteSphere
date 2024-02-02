using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Helpers;

namespace WebApi.ActionFilters
{
    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Class,
        Inherited = true,
        AllowMultiple = false)]
    public class ValidateGuidAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument is Guid guid && guid == Guid.Empty)
                {
                    var status = StatusCodes.Status400BadRequest;

                    var error = new ErrorResponse(
                        HttpStatusHelper.GetTitleByStatusCode(status),
                        "Invalid Guid",
                        status);

                    context.Result = new BadRequestObjectResult(error);
                    return;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
