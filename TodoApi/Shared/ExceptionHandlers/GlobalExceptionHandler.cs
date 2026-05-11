using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Shared.Exceptions;

namespace TodoApi.Shared.ExceptionHandling;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IProblemDetailsService _problemDetailsService;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger,
        IProblemDetailsService problemDetailsService)
    {
        _logger = logger;
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {   
        int statusCode ;
        string title ;
        string type ;

       try{
            throw exception;
       }
       catch(BadRequestException ex){
        statusCode = StatusCodes.Status400BadRequest;
        title = "Bad Request";
        type = "https://tools.ietf.org/html/rfc9110#section-15.5.1";
       }
       catch(ConflictException ex){
        statusCode = StatusCodes.Status409Conflict;
        title = "Conflict";
        type = "https://tools.ietf.org/html/rfc9110#section-15.5.10";
       }
       catch(NotFoundException ex){
        statusCode = StatusCodes.Status404NotFound;
        title = "Not Found";
        type = "https://tools.ietf.org/html/rfc9110#section-15.5.5";
       }
       catch(Exception ex){
        statusCode = StatusCodes.Status500InternalServerError;
        title = "Internal Server Error";
        type = "https://tools.ietf.org/html/rfc9110#section-15.6.1";
       }

        if (statusCode == StatusCodes.Status500InternalServerError)
            _logger.LogError(exception, "An unhandled exception occurred: {ExceptionMessage}", exception.Message);
        else
            _logger.LogWarning(exception, "{Title}: {Message}", title, exception.Message);

        httpContext.Response.StatusCode = statusCode;

        var problemDetailsContext = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
        };

        problemDetailsContext.ProblemDetails.Status = statusCode;
        problemDetailsContext.ProblemDetails.Title = title;
        problemDetailsContext.ProblemDetails.Type = type;
        problemDetailsContext.ProblemDetails.Detail = statusCode != StatusCodes.Status500InternalServerError
            ? exception.Message
            : null;

        return await _problemDetailsService.TryWriteAsync(problemDetailsContext);
    }
}
