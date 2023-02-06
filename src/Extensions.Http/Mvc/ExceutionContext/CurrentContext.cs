using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

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

        public async Task<string> GetTokenAsync()
        {
            return await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
        }

        public string GetByClaimType(string claimType)
        {
            return _httpContextAccessor
                    .HttpContext
                    .User
                    .GetByClaimType(claimType);
        }
    }
}
