using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ThreatLens.Api.Services;

namespace ThreatLens.Api.Infrastructure
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {


            int status;
            string title;

            if (exception is VirusTotalException vt)
            {
                status = vt.StatusCode;
                title = vt.Message;
                _logger.LogWarning(exception, "Scan provider error ({Status})", status);
            }
            else
            {
                status = StatusCodes.Status500InternalServerError;
                title = "An unexpected error occurred."; // it is to not expose the real error 
                _logger.LogError(exception, "Unhandled exception");
            }

            httpContext.Response.StatusCode = status;

            await httpContext.Response.WriteAsJsonAsync(
                new ProblemDetails { Status = status, Title = title },
                options: null,
                contentType: "application/problem+json",
                cancellationToken: cancellationToken);

            return true;
        }
    }
}
