using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Extensions.Http.Mvc
{
    public sealed class ApiErrorEntry
    {
        private ApiErrorEntry() { }

        public ApiErrorEntry(string key, int? errorCode, IEnumerable<string> errors)
        {
            Key = key;
            Errors = errors;
            ErrorCode = errorCode;
        }

        public string Key { get; internal set; }
        public IEnumerable<string> Errors { get; internal set; }
        public int? ErrorCode { get; internal set; }

        public static ApiErrorEntry FormatBadRequest(KeyValuePair<string, ModelStateEntry> input)
        {
            return new ApiErrorEntry
            {
                Key = input.Key,
                ErrorCode = (int)HttpStatusCode.BadRequest,
                Errors = input.Value.Errors.Select(e => e.ErrorMessage)
            };
        }
    }
}
