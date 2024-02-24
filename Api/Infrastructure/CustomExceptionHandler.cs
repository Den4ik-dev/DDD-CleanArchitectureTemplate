using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Infrastructure;

public class CustomExceptionHandler : IExceptionHandler
{
    private readonly ILogger<CustomExceptionHandler> _logger;

    public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        _logger.LogError(exception.Message);

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await context.Response.WriteAsJsonAsync(
            new ProblemDetails()
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal server error",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"
            }
        );

        return true;
    }
}
