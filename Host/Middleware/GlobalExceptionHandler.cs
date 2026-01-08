using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Host.Middleware
{
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var (statusCode, title) = exception switch
            {
                AppException appEx => (appEx.StatusCode, appEx.GetType().Name),
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
                _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
            };

            var problemDetails = new ProblemDetails
            {
                Title = title,
                Status = statusCode,
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }

}
