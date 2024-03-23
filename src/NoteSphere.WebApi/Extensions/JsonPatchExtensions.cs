using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace WebApi.Extensions
{
    public static class JsonPatchExtensions
    {
        public const string ModelStateKey = "Patch";

        public static void ValidateDocument<TModel>(
            this JsonPatchDocument<TModel> jsonPatchDocument,
            ModelStateDictionary modelState) where TModel : class
        {
            if (jsonPatchDocument is null)
            {
                throw new ArgumentNullException($"{nameof(jsonPatchDocument)} is null.");
            }

            if (jsonPatchDocument.Operations.Count == 0)
            {
                modelState.AddModelError(ModelStateKey, "No patch operations provided.");
                return;
            }

            foreach (var opr in jsonPatchDocument.Operations)
            {
                if (string.IsNullOrWhiteSpace(opr.op) || string.IsNullOrWhiteSpace(opr.path))
                {
                    modelState.AddModelError(ModelStateKey, "Some operations are incorrectly formatted or contain null properties/values.");
                    break;
                }
                
               if (opr.OperationType == OperationType.Invalid)
                {
                    modelState.AddModelError(ModelStateKey, $"'{opr.op}' is not a valid operation type.");
                    break;
                }

               if (!opr.path.StartsWith('/'))
                {
                    modelState.AddModelError(ModelStateKey, $"The path '{opr.path}' must start with a forward slash '/'.");
                    break;
                }

               var isInvalidFrom = string.IsNullOrEmpty(opr.from) || !opr.from.StartsWith('/');

               if ((opr.OperationType == OperationType.Copy || opr.OperationType == OperationType.Move)
                    && isInvalidFrom)
                {
                    modelState.AddModelError(ModelStateKey, "Some copy or move operations have invalid 'from' formats. Ensure that 'from' starts with the character '/'.");
                    break;
                }
            }
        }
    }
}
