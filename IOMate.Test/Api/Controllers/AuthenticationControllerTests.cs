using IOMate.Api.Controllers;
using IOMate.Application.Shared.Exceptions;
using IOMate.Application.UseCases.Authentication.Auth;
using IOMate.Application.UseCases.Authentication.Refresh;
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
        var request = new AuthRequestDto("user@email.com", "123456");
        var response = new AuthResponseDto { Token = "jwt-token" };
        _mediatorMock.Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.Auth(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task Refresh_ReturnsOk_WithTokens_WhenValid()
    {
        // Arrange
        var request = new RefreshTokenRequestDto { RefreshToken = "valid-refresh-token" };
        var response = new AuthResponseDto { Token = "access-token", RefreshToken = "refresh-token" };
        _mediatorMock.Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.Refresh(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task Refresh_ReturnsUnauthorized_WhenHandlerThrowsUnauthorized()
    {
        // Arrange
        var request = new RefreshTokenRequestDto { RefreshToken = "invalid-refresh-token" };
        _mediatorMock.Setup(m => m.Send(request, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new UnauthorizedException("Token inválido"));

        // Act
        var exception = await Assert.ThrowsAsync<UnauthorizedException>(() => _controller.Refresh(request));

        // Assert
        Assert.Equal("Token inválido", exception.Message);
    }
}