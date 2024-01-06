using Domain;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Extensions.Http.Mvc.Filters
{
    public class ExceptionHandlerFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            if (exception is EntityNotFoundException notFound)
            {
                context.Result = new ApiResult(HttpStatusCode.NotFound, notFound.Message);
                context.ExceptionHandled = true;
            }
            else if (exception is DomainValidationException validationError)
            {
                context.Result = new ApiResult(HttpStatusCode.BadRequest,
                    new[]
                    {
                        new ApiErrorEntry(validationError.PropertyName ?? "system", new [] { validationError.Message })
                    });
                context.ExceptionHandled = true;
            }
        }
    }
}
