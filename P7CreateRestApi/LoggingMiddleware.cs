using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace P7CreateRestApi
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public LoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<LoggingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation("Request {Method} {Path} started", context.Request.Method, context.Request.Path);

            await _next(context);

            _logger.LogInformation("Request {Method} {Path} completed with status code {StatusCode}",
                context.Request.Method, context.Request.Path, context.Response.StatusCode);
        }

    }

    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }

}
