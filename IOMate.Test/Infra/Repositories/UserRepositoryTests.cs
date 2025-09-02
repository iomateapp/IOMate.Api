using IOMate.Application.Security;
using IOMate.Domain.Entities;
using IOMate.Infra.Repositories;
using Moq;

public class UserRepositoryTests : RepositoryTestBase
{
    [Fact]
    public async Task GetByEmail_ShouldReturnUser_WhenEmailExists()
    {
        // Arrange
        using var context = CreateContext();
        var currentUserContextMock = new Mock<ICurrentUserContext>();
        var user = new User { Email = "test@email.com" };
        context.Users.Add(user);
        await context.SaveChangesAsync();
        var repo = new UserRepository(context, currentUserContextMock.Object);

        // Act
        var result = await repo.GetByEmail("test@email.com", CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Email, result!.Email);
    }

    [Fact]
    public async Task GetByEmail_ShouldReturnNull_WhenEmailDoesNotExist()
    {
        // Arrange
        using var context = CreateContext();
        var currentUserContextMock = new Mock<ICurrentUserContext>();
        var repo = new UserRepository(context, currentUserContextMock.Object);

        // Act
        var result = await repo.GetByEmail("notfound@email.com", CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}