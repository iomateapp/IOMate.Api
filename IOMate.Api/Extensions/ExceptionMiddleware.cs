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
            await HandleValidationExceptionAsync(httpContext, ex);
        }
        catch (BadRequestException ex)
        {
            await HandleBadRequestExceptionAsync(httpContext, ex);
        }
        catch (NotFoundException ex)
        {
            await HandleNotFoundExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var errors = exception.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
        var result = JsonSerializer.Serialize(new { errors });

        return context.Response.WriteAsync(result);
    }

    private static Task HandleBadRequestExceptionAsync(HttpContext context, BadRequestException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var errors = exception.Errors.Select(e => new { PropertyName = string.Empty, ErrorMessage = e });
        var result = JsonSerializer.Serialize(new { errors });

        return context.Response.WriteAsync(result);
    }

    private static Task HandleNotFoundExceptionAsync(HttpContext context, NotFoundException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;

        var errors = new[] { new { PropertyName = string.Empty, ErrorMessage = exception.Message } };
        var result = JsonSerializer.Serialize(new { errors });

        return context.Response.WriteAsync(result);
    }
}