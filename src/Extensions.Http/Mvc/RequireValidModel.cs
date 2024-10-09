using Extensions.Http.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Net;

namespace Extensions.Http.Mvc
{
    public class RequireValidModel : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.ValidationState != ModelValidationState.Valid)
            {
                var data = context.ModelState
                    .Where(i => i.Value.ValidationState != ModelValidationState.Valid)
                    .Select(ApiErrorEntry.FormatBadRequest)
                    .ToArray();

                context.Result = new ApiResult(HttpStatusCode.BadRequest, data);
            }
        }
    }
}
