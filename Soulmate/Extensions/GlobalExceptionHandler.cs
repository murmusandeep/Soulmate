using LoggerService;
using Microsoft.AspNetCore.Diagnostics;
using Shared.ErrorModel;
using Shared.Exceptions;

namespace Soulmate.Extensions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILoggerManager _logger;

        public GlobalExceptionHandler(ILoggerManager logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.ContentType = "application/json";
            var contextFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
            if (contextFeature != null)
            {
                httpContext.Response.StatusCode = contextFeature.Error switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    BadRequestException => StatusCodes.Status400BadRequest,
                    UnAuthorizedException => StatusCodes.Status401Unauthorized,
                    _ => StatusCodes.Status500InternalServerError
                };

                // ✅ Log clean error message
                _logger.LogError($"Something went wrong: {exception.GetType().Name} - {exception.Message}");

                // ✅ Log only first 3 lines of stack trace
                if (!string.IsNullOrWhiteSpace(exception.StackTrace))
                {
                    var traceLines = exception.StackTrace.Split(Environment.NewLine);
                    var shortTrace = string.Join(Environment.NewLine, traceLines.Take(5));
                    _logger.LogError($"Stack Trace (shortened):\n{shortTrace}");
                }

                await httpContext.Response.WriteAsync(new ErrorDetails()
                {
                    StatusCode = httpContext.Response.StatusCode,
                    Message = contextFeature.Error.Message
                }.ToString());
            }
            return true;
        }
    }
}
