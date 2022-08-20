using System;
using System.Linq;
using System.Security.Claims;

namespace Extensions.Http.Mvc
{
    public static class ClaimExtensions
    {
        public static string GetId(this ClaimsPrincipal principal)
        {
            var id = principal.Claims.FirstOrDefault(c => c.Type == "sub");

            if (id == null)
            {
                throw new Exception($"id claim not found, type is sub");
            }

            return id.Value;
        }
    }
}
