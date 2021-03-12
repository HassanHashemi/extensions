using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Extensions.Http.Mvc
{
    public sealed class ApiResult : ActionResult
    {
        public ApiResult(HttpStatusCode statusCode, params ApiErrorEntry[] errors)
        {
            Guard.NotNull(errors, nameof(errors));

            Code = statusCode;
            Errors = errors;
        }

        public ApiResult(HttpStatusCode code, object data)
        {
            Guard.NotNull(data, nameof(data));

            Code = code;
            Data = data;
        }

        public HttpStatusCode Code { get; }
        public IEnumerable<ApiErrorEntry> Errors { get; }
        public object Data { get; }

        public override void ExecuteResult(ActionContext context)
        {
            SetStatusCode(context);

            CreateResult().ExecuteResult(context);
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            SetStatusCode(context);

            return CreateResult().ExecuteResultAsync(context);
        }

        private ObjectResult CreateResult()
        {
            if (IsSuccess(Code))
            {
                return new ObjectResult(Envelop<object>.Success(Code, Data));
            }
            else
            {
                return new ObjectResult(Envelop.HandledError(Code, Errors));
            }

            static bool IsSuccess(HttpStatusCode statusCode) => ((int)statusCode >= 200) && ((int)statusCode <= 299);
        }

        private void SetStatusCode(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)Code;
        }
    }
}
