using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyApi.Middleware
{
    public class ApiKeyMiddleware
    {
        private const string _apiKeyheaderName = "x-api-key";
        private readonly RequestDelegate _next;
        
        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(_apiKeyheaderName, out var apiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized request: required x-api-key header");
                return;
            }

            var envKey = Environment.GetEnvironmentVariable("API_KEY");
            if (!apiKey.Equals(envKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized request: invalid key provided");
                return;
            }
        }
    }
}