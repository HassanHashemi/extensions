using Microsoft.AspNetCore.Http;

namespace Extensions.Http.Mvc
{
    public class CurrentContext : IExecutionContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentContext(IHttpContextAccessor httpContextAccessor) 
            =>_httpContextAccessor = httpContextAccessor;

        public string CurrentUserId =>
            _httpContextAccessor
                    .HttpContext
                    .User
                    .GetId();
    }
}
