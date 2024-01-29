using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Application.Contracts.Common;

namespace TaskManagementSystem.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";
    
        var apiError = new ErrorResponse
        {
            Timestamp = DateTime.Now,
            Errors = new List<string> { exception.Message }
        };
    
        switch (exception)
        {
            case ApplicationException:
                apiError.StatusCode = (int)HttpStatusCode.BadRequest;
                apiError.StatusPhrase = "Bad Request";
                break;
            case KeyNotFoundException:
                apiError.StatusCode = (int)HttpStatusCode.NotFound;
                apiError.StatusPhrase = "Not Found";
                break;
            case UnauthorizedAccessException:
                apiError.StatusCode = (int)HttpStatusCode.Forbidden;
                apiError.StatusPhrase = "Not Allowed to change";
                break;
            case DbUpdateException:
                apiError.StatusCode = (int)HttpStatusCode.BadRequest;
                apiError.StatusPhrase = "Error while updating the database";
                break;
            case ArgumentException:
                apiError.StatusCode = (int)HttpStatusCode.BadRequest;
                apiError.StatusPhrase = "Argument issue";
                break;
            case ArithmeticException:
                apiError.StatusCode = (int)HttpStatusCode.BadRequest;
                apiError.StatusPhrase = "Arithmetic Exception";
                break;
            case TimeoutException:
                apiError.StatusCode = (int)HttpStatusCode.RequestTimeout;
                apiError.StatusPhrase = "Timeout Exception";
                break;
            default:
                apiError.StatusCode = (int)HttpStatusCode.InternalServerError;
                apiError.StatusPhrase = "Internal Server Error";
                break;
        }
    
        _logger.LogError(exception, JsonSerializer.Serialize(apiError));
    
        response.StatusCode = apiError.StatusCode;
        await response.WriteAsync(JsonSerializer.Serialize(apiError));
    }
}