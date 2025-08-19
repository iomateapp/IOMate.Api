using IOMate.Infra.Context;
using Microsoft.EntityFrameworkCore;

public abstract class RepositoryTestBase
{
    protected AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }
}