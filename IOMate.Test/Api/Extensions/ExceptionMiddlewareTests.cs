using FluentValidation;
using FluentValidation.Results;
using IOMate.Application.Resources;
using IOMate.Application.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Moq;
using System.Net;
using System.Text.Json;

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
            new ValidationFailure("Email", "Email inv�lido"),
            new ValidationFailure("Password", "Senha obrigat�ria")
        };
        var exception = new ValidationException(validationFailures);

        var context = CreateContextWithBody();
        var localizerMock = new Mock<IStringLocalizer<Messages>>();
        var middleware = new ExceptionMiddleware(_ => throw exception, localizerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
        var json = await GetResponseJson(context.Response);
        var errors = json.GetProperty("ValidationErrors").EnumerateArray().ToList();
        Assert.Contains(errors, e => e.GetProperty("Field").GetString() == "Email");
        Assert.Contains(errors, e => e.GetProperty("Field").GetString() == "Password");

        Assert.Equal("application/json", context.Response.ContentType);
    }

    [Fact]
    public async Task InvokeAsync_Should_Handle_BadRequestException()
    {
        // Arrange
        var error = "Error message";
        var exception = new BadRequestException(error);

        var context = CreateContextWithBody();
        var localizerMock = new Mock<IStringLocalizer<Messages>>();
        var middleware = new ExceptionMiddleware(_ => throw exception, localizerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
        var json = await GetResponseJson(context.Response);
        var errorList = json.GetProperty("ValidationErrors").EnumerateArray().ToList();
        Assert.Empty(errorList);
        Assert.Equal(error, json.GetProperty("Message").GetString());
        Assert.Equal("application/json", context.Response.ContentType);
    }

    [Fact]
    public async Task InvokeAsync_Should_Handle_NotFoundException()
    {
        // Arrange
        var exception = new NotFoundException("Usu�rio n�o encontrado");

        var context = CreateContextWithBody();
        var localizerMock = new Mock<IStringLocalizer<Messages>>();
        var middleware = new ExceptionMiddleware(_ => throw exception, localizerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal((int)HttpStatusCode.NotFound, context.Response.StatusCode);
        var json = await GetResponseJson(context.Response);
        var errors = json.GetProperty("ValidationErrors").EnumerateArray().ToList();
        Assert.Empty(errors);
        Assert.Equal("Usu�rio n�o encontrado", json.GetProperty("Message").GetString());
        Assert.Equal("application/json", context.Response.ContentType);
    }

    [Fact]
    public async Task InvokeAsync_Should_Call_Next_When_No_Exception()
    {
        // Arrange
        var context = CreateContextWithBody();
        var wasCalled = false;
        var localizerMock = new Mock<IStringLocalizer<Messages>>();
        var middleware = new ExceptionMiddleware(_ =>
        {
            wasCalled = true;
            return Task.CompletedTask;
        }, localizerMock.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.True(wasCalled);
        Assert.Equal(200, context.Response.StatusCode);
    }
}