using FluentValidation;
using IOMate.Application.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ValidationException ex)
        {
            await WriteErrorResponseAsync(httpContext, HttpStatusCode.BadRequest,
                ex.Errors.Select(e => new { Field = e.PropertyName, Message = e.ErrorMessage }),
                "Validation failed.");
        }
        catch (BadRequestException ex)
        {
            await WriteErrorResponseAsync(httpContext, HttpStatusCode.BadRequest,
                Enumerable.Empty<object>(),
                ex.Message ?? "Bad request.");
        }
        catch (NotFoundException ex)
        {
            await WriteErrorResponseAsync(httpContext, HttpStatusCode.NotFound,
                Enumerable.Empty<object>(),
                ex.Message ?? "Resource not found.");
        }
        catch (Exception ex)
        {
            await WriteErrorResponseAsync(httpContext, HttpStatusCode.InternalServerError,
                Enumerable.Empty<object>(),
                "An unexpected error occurred.");
        }
    }

    private static Task WriteErrorResponseAsync(HttpContext context, HttpStatusCode statusCode, IEnumerable<object> validationErrors, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var result = JsonSerializer.Serialize(new
        {
            ValidationErrors = validationErrors.ToList(),
            Message = message
        });

        return context.Response.WriteAsync(result);
    }
}