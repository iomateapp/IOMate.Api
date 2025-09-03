using IOMate.Api.Extensions;
using IOMate.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using System.Text.Json;

public class CurrentUserContextTests
{
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();
    private readonly CurrentUserContext _currentUserContext;

    public CurrentUserContextTests()
    {
        _currentUserContext = new CurrentUserContext(_httpContextAccessorMock.Object);
    }

    [Fact]
    public void User_ReturnsUser_WhenUserClaimIsValid()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com"
        };
        var userJson = JsonSerializer.Serialize(user);

        var claims = new List<Claim> { new Claim("user", userJson) };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(c => c.User).Returns(principal);

        _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);

        // Act
        var result = _currentUserContext.User;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result!.Id);
        Assert.Equal(user.FirstName, result.FirstName);
        Assert.Equal(user.LastName, result.LastName);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public void User_ReturnsNull_WhenNoHttpContext()
    {
        // Arrange
        _httpContextAccessorMock.Setup(a => a.HttpContext).Returns((HttpContext)null!);

        // Act
        var result = _currentUserContext.User;

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void User_ReturnsNull_WhenNoUserClaim()
    {
        // Arrange
        var claims = new List<Claim>();
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(c => c.User).Returns(principal);

        _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);

        // Act
        var result = _currentUserContext.User;

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void User_ReturnsNull_WhenUserClaimIsInvalid()
    {
        // Arrange
        var claims = new List<Claim> { new Claim("user", "invalid-json") };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(c => c.User).Returns(principal);

        _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);

        // Act
        var result = _currentUserContext.User;

        // Assert
        Assert.Null(result);
    }
}