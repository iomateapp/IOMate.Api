using FluentValidation;
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
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            var result = JsonSerializer.Serialize(new { errors });

            await httpContext.Response.WriteAsync(result);
        }
    }
}