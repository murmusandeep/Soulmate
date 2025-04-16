using Microsoft.AspNetCore.Http;

namespace LoggerService
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILoggerManager logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            var info = $"[Request] HTTP {request.Method} - {request.Path}";

            _logger.LogInfo(info);

            // Log headers (optional)
            foreach (var header in request.Headers)
            {
                _logger.LogDebug($"Header: {header.Key} = {header.Value}");
            }

            await _next(context);
        }
    }

}
