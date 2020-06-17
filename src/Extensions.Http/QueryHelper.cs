using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Extensions.Http
{
    public static class QueryHelper
    {
        public static string ToQueryString(this object model)
        {
            var result = new StringBuilder();

            foreach (var property in model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = property.GetValue(model);

                if (value == null)
                {
                    continue;
                }

                result
                    .Append(property.Name)
                    .Append("=")
                    .Append(value.ToString())
                    .Append("&");
            }

            return result.ToString();
        }
    }
}
