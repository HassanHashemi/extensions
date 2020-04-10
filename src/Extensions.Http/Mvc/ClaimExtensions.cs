using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Extensions.Http.Mvc
{
    public static class ClaimExtensions
    {
        public static Guid GetId(this ClaimsPrincipal principal)
        {
            var id = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (id == null)
            {
                throw new Exception($"id claim not found, type is {ClaimTypes.NameIdentifier}");
            }

            return Guid.Parse(id.Value);
        }
    }
}
