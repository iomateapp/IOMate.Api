using IOMate.Application.Services;
using Xunit;

public class PasswordHasherTests
{
    private readonly PasswordHasher _hasher = new();

    [Fact]
    public void HashPassword_ShouldReturn_Hash()
    {
        var password = "MySecret123!";
        var hash = _hasher.HashPassword(password);

        Assert.False(string.IsNullOrWhiteSpace(hash));
        Assert.NotEqual(password, hash);
    }

    [Fact]
    public void VerifyPassword_ShouldReturn_True_For_ValidPassword()
    {
        var password = "MySecret123!";
        var hash = _hasher.HashPassword(password);

        var result = _hasher.VerifyPassword(password, hash);

        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_ShouldReturn_False_For_InvalidPassword()
    {
        var password = "MySecret123!";
        var hash = _hasher.HashPassword(password);

        var result = _hasher.VerifyPassword("WrongPassword", hash);

        Assert.False(result);
    }
}