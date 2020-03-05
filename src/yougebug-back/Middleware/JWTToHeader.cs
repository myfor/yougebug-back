using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using yougebug_back.Shared;

namespace yougebug_back.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class JWTToHeader
    {
        private readonly RequestDelegate _next;

        public JWTToHeader(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Cookies.TryGetValue(Defaults.ADMIN_AUTH_COOKIE_KEY, out string jwt))
            {
                httpContext.Request.Headers.Add("Authorization", $"Bearer {jwt}");
            }
            else if (httpContext.Request.Cookies.TryGetValue(Defaults.CLIENT_AUTH_COOKIE_KEY, out jwt))
            {
                httpContext.Request.Headers.Add("Authorization", $"Bearer {jwt}");
            }
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class JWTToHeaderExtensions
    {
        public static IApplicationBuilder UseJWTToHeader(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JWTToHeader>();
        }
    }
}
