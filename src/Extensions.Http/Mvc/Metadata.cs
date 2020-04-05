using System.Collections.Generic;
using System.Net;

namespace Extensions.Http.Mvc
{
    public class Metadata
    {
        public HttpStatusCode Code { get; set; }
        public string ErrorMessage { get; set; }
        public IEnumerable<ApiErrorEntry> Errors { get; set; }
    }
}
