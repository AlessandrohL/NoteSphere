using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.Extensions;

namespace WebApi.ActionFilters
{
    [AttributeUsage(
        AttributeTargets.Method,
        AllowMultiple = false)]
    public class ValidatePatchDocument<TModel> : ActionFilterAttribute where TModel : class
    {
        public override async Task OnActionExecutionAsync(
            ActionExecutingContext context, 
            ActionExecutionDelegate next)
        {
            foreach (var item in context.ActionArguments)
            {
                if (item.Value is JsonPatchDocument<TModel> document)
                {
                    document.ValidateDocument(context.ModelState);

                    if (!context.ModelState.IsValid)
                    {
                        context.Result = new BadRequestObjectResult(
                            new ValidationProblemDetails(context.ModelState));
                        return;
                    }
                }
            }

            await next();
        }
    }
}
