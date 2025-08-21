using FluentValidation;
using IOMate.Application.Resources;
using IOMate.Application.Shared.Exceptions;
using Microsoft.Extensions.Localization;
using System.Net;
using System.Text.Json;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IStringLocalizer<Messages> _localizer;

    public ExceptionMiddleware(RequestDelegate next, IStringLocalizer<Messages> localizer)
    {
        _next = next;
        _localizer = localizer;
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
                _localizer["ValidationFailed"]);
        }
        catch (BadRequestException ex)
        {
            await WriteErrorResponseAsync(httpContext, HttpStatusCode.BadRequest,
                Enumerable.Empty<object>(),
                ex.Message ?? _localizer["BadRequest"]);
        }
        catch (NotFoundException ex)
        {
            await WriteErrorResponseAsync(httpContext, HttpStatusCode.NotFound,
                Enumerable.Empty<object>(),
                ex.Message ?? _localizer["ResourceNotFound"]);
        }
        catch (Exception)
        {
            await WriteErrorResponseAsync(httpContext, HttpStatusCode.InternalServerError,
                Enumerable.Empty<object>(),
                _localizer["UnexpectedError"]);
        }
    }

    private Task WriteErrorResponseAsync(HttpContext context, HttpStatusCode statusCode, IEnumerable<object> validationErrors, string message)
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