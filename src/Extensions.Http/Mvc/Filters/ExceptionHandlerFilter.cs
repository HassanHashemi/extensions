using Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using MongoDB.Driver.Core.WireProtocol.Messages;
using System;
using System.Net;

namespace Extensions.Http.Mvc.Filters
{
    public class ExceptionHandlerFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _environment;

        public ExceptionHandlerFilter(IHostingEnvironment environment)
        {
            this._environment = environment;
        }

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
            else
            {
                var defaultMessage = "Internal Error, Please contact support";

                var message = _environment.IsProduction() ? exception.ToString() : defaultMessage;

                context.Result = new ApiResult(HttpStatusCode.InternalServerError,
                    new[]
                    {
                        new ApiErrorEntry("system", new [] { message })
                    });

                context.ExceptionHandled = true;
            }
        }
    }
}
