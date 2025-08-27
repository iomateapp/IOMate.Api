using IOMate.Application.Services;
using IOMate.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class JwtTokenGeneratorTests
{
    [Fact]
    public void GenerateToken_ShouldReturn_ValidToken()
    {
        // Arrange
        var inMemorySettings = new Dictionary<string, string>
        {
            {"JwtSettings:Secret", "supersecretkeysupersecretkey123!"},
            {"JwtSettings:Issuer", "TestIssuer"},
            {"JwtSettings:Audience", "TestAudience"},
            {"JwtSettings:ExpirationMinutes", "60"}
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
        var generator = new JwtTokenGenerator(configuration);
        var user = new User { Id = Guid.NewGuid(), Email = "user@test.com" };

        // Act
        var token = generator.GenerateToken(user);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(token));
        Assert.Contains(".", token);
    }

    [Fact]
    public void GenerateRefreshToken_ShouldReturn_ValidRefreshToken_WithTypRefresh()
    {
        // Arrange
        var inMemorySettings = new Dictionary<string, string>
        {
            {"JwtSettings:Secret", "supersecretkeysupersecretkey123!"},
            {"JwtSettings:Issuer", "TestIssuer"},
            {"JwtSettings:Audience", "TestAudience"},
            {"JwtSettings:ExpirationMinutes", "60"}
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
        var generator = new JwtTokenGenerator(configuration);
        var user = new User { Id = Guid.NewGuid(), Email = "user@test.com" };

        // Act
        var refreshToken = generator.GenerateRefreshToken(user);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(refreshToken));
        Assert.Contains(".", refreshToken);

        // Decodifica o token para garantir que a claim "typ" é "refresh"
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(refreshToken);
        var typClaim = jwt.Claims.FirstOrDefault(c => c.Type == "typ")?.Value;
        Assert.Equal("refresh", typClaim);
    }

    [Fact]
    public void GetPrincipalFromExpiredToken_ShouldReturnPrincipal_WhenTokenIsValid()
    {
        // Arrange
        var inMemorySettings = new Dictionary<string, string>
        {
            {"JwtSettings:Secret", "supersecretkeysupersecretkey123!"},
            {"JwtSettings:Issuer", "TestIssuer"},
            {"JwtSettings:Audience", "TestAudience"},
            {"JwtSettings:ExpirationMinutes", "60"}
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
        var generator = new JwtTokenGenerator(configuration);
        var user = new User { Id = Guid.NewGuid(), Email = "user@test.com" };
        var refreshToken = generator.GenerateRefreshToken(user);

        // Act
        var principal = generator.GetPrincipalFromExpiredToken(refreshToken, isRefreshToken: true);

        // Assert
        Assert.NotNull(principal);
        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Assert.Equal(user.Id.ToString(), userId);
        Assert.Equal(user.Email, principal.FindFirst(ClaimTypes.Email)?.Value);
        Assert.Equal("refresh", principal.FindFirst("typ")?.Value);
    }
}