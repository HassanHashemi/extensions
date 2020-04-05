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

            this.Code = statusCode;
            this.Errors = errors;
        }

        public ApiResult(HttpStatusCode code, object data)
        {
            Guard.NotNull(data, nameof(data));

            this.Code = code;
            this.Data = data;
        }

        public HttpStatusCode Code { get; }
        public IEnumerable<ApiErrorEntry> Errors { get; }
        public object Data { get; }

        public override void ExecuteResult(ActionContext context)
        {
            SetStatusCode(context);

            this.CreateResult().ExecuteResult(context);
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            SetStatusCode(context);

            return this.CreateResult().ExecuteResultAsync(context);
        }

        private ObjectResult CreateResult()
        {
            if (IsSuccess(this.Code))
            {
                return new ObjectResult(Envelop.Success(this.Code, this.Data));
            }
            else
            {
                return new ObjectResult(Envelop.HandledError(this.Code, this.Errors));
            }

            static bool IsSuccess(HttpStatusCode statusCode) => ((int)statusCode >= 200) && ((int)statusCode <= 299);
        }

        private void SetStatusCode(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)this.Code;
        }
    }
}
