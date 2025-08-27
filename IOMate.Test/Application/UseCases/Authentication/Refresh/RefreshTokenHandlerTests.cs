using IOMate.Application.UseCases.Authentication.Refresh;
using IOMate.Domain.Entities;
using IOMate.Domain.Interfaces;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class RefreshTokenHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnNewTokens_WhenRefreshTokenIsValid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, Email = "user@test.com" };
        var refreshToken = "valid-refresh-token";
        var newAccessToken = "new-access-token";
        var newRefreshToken = "new-refresh-token";

        var jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        jwtTokenGeneratorMock
            .Setup(j => j.GetPrincipalFromExpiredToken(refreshToken, true))
            .Returns(new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString())
            })));
        jwtTokenGeneratorMock.Setup(j => j.GenerateToken(user)).Returns(newAccessToken);
        jwtTokenGeneratorMock.Setup(j => j.GenerateRefreshToken(user)).Returns(newRefreshToken);

        var userRepositoryMock = new Mock<IUserRepository>();
        userRepositoryMock.Setup(r => r.GetByIdAsync(userId, default)).ReturnsAsync(user);

        var handler = new RefreshTokenHandler(jwtTokenGeneratorMock.Object, userRepositoryMock.Object);

        var request = new RefreshTokenRequestDto { RefreshToken = refreshToken };

        // Act
        var result = await handler.Handle(request, default);

        // Assert
        Assert.Equal(newAccessToken, result.Token);
        Assert.Equal(newRefreshToken, result.RefreshToken);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenRefreshTokenIsInvalid()
    {
        // Arrange
        var jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        jwtTokenGeneratorMock
            .Setup(j => j.GetPrincipalFromExpiredToken(It.IsAny<string>(), true))
            .Returns((ClaimsPrincipal?)null);

        var userRepositoryMock = new Mock<IUserRepository>();
        var handler = new RefreshTokenHandler(jwtTokenGeneratorMock.Object, userRepositoryMock.Object);

        var request = new RefreshTokenRequestDto { RefreshToken = "invalid-token" };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(request, default));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenUserNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var refreshToken = "valid-refresh-token";

        var jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        jwtTokenGeneratorMock
            .Setup(j => j.GetPrincipalFromExpiredToken(refreshToken, true))
            .Returns(new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString())
            })));

        var userRepositoryMock = new Mock<IUserRepository>();
        userRepositoryMock.Setup(r => r.GetByIdAsync(userId, default)).ReturnsAsync((User?)null);

        var handler = new RefreshTokenHandler(jwtTokenGeneratorMock.Object, userRepositoryMock.Object);

        var request = new RefreshTokenRequestDto { RefreshToken = refreshToken };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(request, default));
    }
}