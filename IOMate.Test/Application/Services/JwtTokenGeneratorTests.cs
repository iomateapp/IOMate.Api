using IOMate.Application.Services;
using IOMate.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Xunit;

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
}