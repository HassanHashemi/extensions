using DnsClient.Internal;
using Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MongoDB.Driver.Core.WireProtocol.Messages;
using System;
using System.Net;

namespace Extensions.Http.Mvc.Filters
{
    public class ExceptionHandlerFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _environment;
        private readonly ILogger<ExceptionHandlerFilter> _logger;

        public ExceptionHandlerFilter(IHostingEnvironment environment, ILogger<ExceptionHandlerFilter> logger)
        {
            this._environment = environment;
            this._logger = logger;
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
                _logger.LogError(exception.ToString());

                var defaultMessage = "Internal Error, Please contact support";

                var message = !_environment.IsProduction() ? exception.ToString() : defaultMessage;

                context.Result = new ApiResult(HttpStatusCode.InternalServerError,
                    new[]
                    {
                        new ApiErrorEntry("system", new [] { message })
                    });
            }
        }
    }
}
