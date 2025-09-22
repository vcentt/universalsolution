using UniversalSolution.Domain.DTOs;
using UniversalSolution.Middleware.Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (FluentValidation.ValidationException vex)
        {
            _logger.LogWarning(vex, "Validation error");

            var errors = vex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            await WriteResponseAsync(context, StatusCodes.Status400BadRequest, "Validation failed", errors);
        }
        catch (CustomException cex)
        {
            _logger.LogWarning(cex, "Custom exception handled");
            await WriteResponseAsync(context, cex.StatusCode, cex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteResponseAsync(context, StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }

    private static async Task WriteResponseAsync(HttpContext context, int statusCode, string message, Dictionary<string, string[]>? errors = null)
    {
        var response = new ErrorResponse
        {
            StatusCode = statusCode,
            Message = message,
            Errors = errors
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(response);
    }
}