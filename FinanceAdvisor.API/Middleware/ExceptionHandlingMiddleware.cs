using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FinanceAdvisor.API.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger
)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        } catch ( Exception exception)
        {
            logger.LogError(exception, "Unhandled exception");

            await HandleExceptionAsync(context, exception);           
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception
    )
    {
        var (statusCode, title) = exception switch
    {
        ArgumentException => (
            StatusCodes.Status400BadRequest,
            "Invalid request"
        ),

        InvalidOperationException => (
            StatusCodes.Status409Conflict,
            "Invalid operation"
        ),

        KeyNotFoundException => (
            StatusCodes.Status404NotFound,
            "Resource not found"
        ),

        _ => (
            StatusCodes.Status500InternalServerError,
            "Unexpected error"
        )
    };

    var problem = new ProblemDetails
    {
        Status = statusCode,
        Title = title,
        Detail = exception.Message,
        Instance = context.Request.Path
    };

    
    context.Response.StatusCode = statusCode;
    context.Response.ContentType = "application/problem+json";
    await JsonSerializer.SerializeAsync(
    context.Response.Body,
    problem,
    new JsonSerializerOptions(JsonSerializerDefaults.Web)
);
    }
}