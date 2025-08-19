using IOMate.Domain.Entities;
using IOMate.Infra.Context;
using IOMate.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class UserRepositoryTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetByEmail_ShouldReturnUser_WhenEmailExists()
    {
        using var context = CreateContext();
        var user = new User { Email = "test@email.com" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repo = new UserRepository(context);

        var result = await repo.GetByEmail("test@email.com", CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(user.Email, result!.Email);
    }

    [Fact]
    public async Task GetByEmail_ShouldReturnNull_WhenEmailDoesNotExist()
    {
        using var context = CreateContext();
        var repo = new UserRepository(context);

        var result = await repo.GetByEmail("notfound@email.com", CancellationToken.None);

        Assert.Null(result);
    }
}