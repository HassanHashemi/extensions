using System.Collections.Generic;
using System.Net;

namespace Extensions.Http.Mvc
{
    public class Envelop<T>
    {
        public Envelop() { }

        public Metadata Meta { get; set; }
        public T Data { get; set; }
        public PaginationInfo Pagination { get; set; }

        public static Envelop HandledError(HttpStatusCode code, IEnumerable<ApiErrorEntry> errors, string errorMessage = null)
        {
            return new Envelop
            {
                Meta = new Metadata
                {
                    Code = code,
                    Errors = errors,
                    ErrorMessage = errorMessage
                }
            };
        }

        public static Envelop<TData> Success<TData>(HttpStatusCode code, TData data, PaginationInfo pagination = null)
        {
            return new Envelop<TData>
            {
                Meta = new Metadata { Code = code },
                Data = data,
                Pagination = pagination
            };
        }
    }
}
