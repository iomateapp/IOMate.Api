using AutoMapper;
using IOMate.Application.Shared.Exceptions;
using IOMate.Application.UseCases.Authentication.Auth;
using IOMate.Domain.Entities;
using IOMate.Domain.Interfaces;
using Moq;

public class AuthHandlerTests : BaseTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock = new();

    [Fact]
    public async Task Handle_ShouldThrow_WhenUserNotFound()
    {
        // Arrange
        var request = new AuthRequestDto("notfound@email.com", "123456");
        _userRepositoryMock.Setup(r => r.GetByEmail(request.Email, default)).ReturnsAsync((User)null!);
        var handler = new AuthHandler(
            _userRepositoryMock.Object,
            _mapperMock.Object,
            _passwordHasherMock.Object,
            _jwtTokenGeneratorMock.Object,
            Localizer);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(request, default));
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenPasswordInvalid()
    {
        // Arrange
        var user = new User { Email = "user@email.com", Password = "hashed" };
        var request = new AuthRequestDto(user.Email, "wrongpass");
        _userRepositoryMock.Setup(r => r.GetByEmail(request.Email, default)).ReturnsAsync(user);
        _passwordHasherMock.Setup(h => h.VerifyPassword(request.Password, user.Password)).Returns(false);
        var handler = new AuthHandler(
            _userRepositoryMock.Object,
            _mapperMock.Object,
            _passwordHasherMock.Object,
            _jwtTokenGeneratorMock.Object,
            Localizer);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(request, default));
    }

    [Fact]
    public async Task Handle_ShouldReturnToken_WhenValid()
    {
        // Arrange
        var user = new User { Email = "user@email.com", Password = "hashed" };
        var request = new AuthRequestDto(user.Email, "123456");
        _userRepositoryMock.Setup(r => r.GetByEmail(request.Email, default)).ReturnsAsync(user);
        _passwordHasherMock.Setup(h => h.VerifyPassword(request.Password, user.Password)).Returns(true);
        _jwtTokenGeneratorMock.Setup(j => j.GenerateToken(user)).Returns("token");
        var handler = new AuthHandler(
            _userRepositoryMock.Object,
            _mapperMock.Object,
            _passwordHasherMock.Object,
            _jwtTokenGeneratorMock.Object,
            Localizer);

        // Act
        var result = await handler.Handle(request, default);

        // Assert
        Assert.Equal("token", result.Token);
    }
}