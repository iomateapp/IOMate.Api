using IOMate.Api.Controllers;
using IOMate.Application.UseCases.Authentication.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

public class AuthenticationControllerTests
{
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly AuthenticationController _controller;

    public AuthenticationControllerTests()
    {
        _controller = new AuthenticationController(_mediatorMock.Object);
    }

    [Fact]
    public async Task Authenticate_ReturnsOk_WithToken()
    {
        // Arrange
        var request = new AuthenticationRequestDto("user@email.com", "123456");
        var response = new AuthResponseDto { Token = "jwt-token" };
        _mediatorMock.Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.Auth(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
    }
}