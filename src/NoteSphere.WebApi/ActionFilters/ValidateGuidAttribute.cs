using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

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
                    context.Result = new BadRequestObjectResult("Invalid Guid");
                    return;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
