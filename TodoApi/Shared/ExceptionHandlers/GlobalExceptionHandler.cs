using TodoApi.Shared.Exceptions;

namespace TodoApi.Shared.ExceptionHandler;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;

    }
    public async Task InvokeAsync(HttpContext context, IProblemDetailsService problemDetailsService)
    {
        int statusCode;
        string title;
        string type;
        Exception? capturedException = null;

        try
        {
            await _next(context);
            return;
        }
        catch (BadRequestException ex)
        {
            capturedException = ex;
            statusCode = StatusCodes.Status400BadRequest;
            title = "Bad Request";
            type = "https://tools.ietf.org/html/rfc9110#section-15.5.1";
        }
        catch (ConflictException ex)
        {
            capturedException = ex;
            statusCode = StatusCodes.Status409Conflict;
            title = "Conflict";
            type = "https://tools.ietf.org/html/rfc9110#section-15.5.10";
        }
        catch (NotFoundException ex)
        {
            capturedException = ex;
            statusCode = StatusCodes.Status404NotFound;
            title = "Not Found";
            type = "https://tools.ietf.org/html/rfc9110#section-15.5.5";
        }
        catch (Exception ex)
        {
            capturedException = ex;
            statusCode = StatusCodes.Status500InternalServerError;
            title = "Internal Server Error";
            type = "https://tools.ietf.org/html/rfc9110#section-15.6.1";
            _logger.LogError(ex, "An unhandled exception occurred");
        }

        context.Response.StatusCode = statusCode;

        var problemDetailsContext = new ProblemDetailsContext
        {
            HttpContext = context,

            Exception = capturedException
        };

        problemDetailsContext.ProblemDetails.Status = statusCode;
        problemDetailsContext.ProblemDetails.Title = title;
        problemDetailsContext.ProblemDetails.Type = type;

        if (statusCode != StatusCodes.Status500InternalServerError)
        {
            problemDetailsContext.ProblemDetails.Detail = capturedException?.Message;

        }
        await problemDetailsService.TryWriteAsync(problemDetailsContext);
    }

}