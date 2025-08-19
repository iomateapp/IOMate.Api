using System.Net;
using System.Text.Json;
using FluentValidation;
using FluentValidation.Results;
using IOMate.Api.Extensions;
using IOMate.Application.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Xunit;

public class ExceptionMiddlewareTests
{
    private static DefaultHttpContext CreateContextWithBody()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        return context;
    }

    private static async Task<JsonElement> GetResponseJson(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(response.Body).ReadToEndAsync();
        return JsonSerializer.Deserialize<JsonElement>(responseBody);
    }

    [Fact]
    public async Task InvokeAsync_Should_Handle_ValidationException()
    {
        // Arrange
        var validationFailures = new[]
        {
            new ValidationFailure("Email", "Email inválido"),
            new ValidationFailure("Password", "Senha obrigatória")
        };
        var exception = new ValidationException(validationFailures);

        var context = CreateContextWithBody();
        var middleware = new ExceptionMiddleware(_ => throw exception);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
        var json = await GetResponseJson(context.Response);
        var errors = json.GetProperty("errors").EnumerateArray().ToList();
        Assert.Contains(errors, e => e.GetProperty("PropertyName").GetString() == "Email" && e.GetProperty("ErrorMessage").GetString() == "Email inválido");
        Assert.Contains(errors, e => e.GetProperty("PropertyName").GetString() == "Password" && e.GetProperty("ErrorMessage").GetString() == "Senha obrigatória");
        Assert.Equal("application/json", context.Response.ContentType);
    }

    [Fact]
    public async Task InvokeAsync_Should_Handle_BadRequestException()
    {
        // Arrange
        var errors = new[] { "Campo obrigatório", "Formato inválido" };
        var exception = new BadRequestException(errors);

        var context = CreateContextWithBody();
        var middleware = new ExceptionMiddleware(_ => throw exception);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
        var json = await GetResponseJson(context.Response);
        var errorList = json.GetProperty("errors").EnumerateArray().ToList();
        Assert.Contains(errorList, e => e.GetProperty("ErrorMessage").GetString() == "Campo obrigatório");
        Assert.Contains(errorList, e => e.GetProperty("ErrorMessage").GetString() == "Formato inválido");
        Assert.Equal("application/json", context.Response.ContentType);
    }

    [Fact]
    public async Task InvokeAsync_Should_Handle_NotFoundException()
    {
        // Arrange
        var exception = new NotFoundException("Usuário não encontrado");

        var context = CreateContextWithBody();
        var middleware = new ExceptionMiddleware(_ => throw exception);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal((int)HttpStatusCode.NotFound, context.Response.StatusCode);
        var json = await GetResponseJson(context.Response);
        var errors = json.GetProperty("errors").EnumerateArray().ToList();
        Assert.Single(errors);
        Assert.Equal("Usuário não encontrado", errors[0].GetProperty("ErrorMessage").GetString());
        Assert.Equal("application/json", context.Response.ContentType);
    }

    [Fact]
    public async Task InvokeAsync_Should_Call_Next_When_No_Exception()
    {
        // Arrange
        var context = CreateContextWithBody();
        var wasCalled = false;
        var middleware = new ExceptionMiddleware(_ =>
        {
            wasCalled = true;
            return Task.CompletedTask;
        });

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.True(wasCalled);
        Assert.Equal(200, context.Response.StatusCode);
    }
}