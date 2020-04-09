using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace Extensions.Http.Mvc
{
    public abstract class BaseController : Controller
    {
        protected IActionResult ApiOk(object data)
        {
            if (data.GetType().IsPrimitive || data is string)
            {
                throw new ArgumentException("Primitive types are not supported");
            }

            return new ApiResult(HttpStatusCode.OK, data);
        }

        protected IActionResult BadRequestInternal(string error)
        {
            var errorEntry = new ApiErrorEntry("message", new[] { error });
            return BadRequestInternal(errorEntry);
        }

        protected IActionResult BadRequestInternal(params ApiErrorEntry[] errors)
        {
            return new ApiResult(HttpStatusCode.BadRequest, errors);
        }
    }
}
