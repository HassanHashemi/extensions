using System.Collections.Generic;
using System.Net;

namespace Extensions.Http.Mvc
{
    public sealed class Envelop
    {
        private Envelop() { }

        public Metadata Meta { get; private set; }
        public object Data { get; private set; }
        public PaginationInfo Pagination { get; set; }

        public static Envelop HandledError(HttpStatusCode code, IEnumerable<ApiErrorEntry> errors)
        {
            return new Envelop()
            {
                Meta = new Metadata { Code = code, Errors = errors }
            };
        }

        public static Envelop Success(HttpStatusCode code, object data, PaginationInfo pagination = null)
        {
            return new Envelop()
            {
                Meta = new Metadata { Code = code },
                Data = data,
                Pagination = pagination
            };
        }
    }
}
