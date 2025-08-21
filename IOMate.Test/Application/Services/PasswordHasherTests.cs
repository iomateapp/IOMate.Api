using IOMate.Application.Services;

public class PasswordHasherTests
{
    private readonly PasswordHasher _hasher = new();

    [Fact]
    public void HashPassword_ShouldReturn_Hash()
    {
        // Arrange
        var password = "MySecret123!";

        // Act
        var hash = _hasher.HashPassword(password);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(hash));
        Assert.NotEqual(password, hash);
    }

    [Fact]
    public void VerifyPassword_ShouldReturn_True_For_ValidPassword()
    {
        // Arrange
        var password = "MySecret123!";
        var hash = _hasher.HashPassword(password);

        // Act
        var result = _hasher.VerifyPassword(password, hash);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_ShouldReturn_False_For_InvalidPassword()
    {
        // Arrange
        var password = "MySecret123!";
        var hash = _hasher.HashPassword(password);

        // Act
        var result = _hasher.VerifyPassword("WrongPassword", hash);

        // Assert
        Assert.False(result);
    }
}