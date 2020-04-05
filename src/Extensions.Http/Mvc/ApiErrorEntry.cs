using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace Extensions.Http.Mvc
{
    public sealed class ApiErrorEntry
    {
        private ApiErrorEntry() { }

        public ApiErrorEntry(string key, IEnumerable<string> errors)
        {
            Key = key;
            Errors = errors;
        }

        public string Key { get; internal set; }
        public IEnumerable<string> Errors { get; internal set; }

        public static ApiErrorEntry Format(KeyValuePair<string, ModelStateEntry> input)
        {
            return new ApiErrorEntry
            {
                Key = input.Key,
                Errors = input.Value.Errors.Select(e => e.ErrorMessage)
            };
        }
    }
}
