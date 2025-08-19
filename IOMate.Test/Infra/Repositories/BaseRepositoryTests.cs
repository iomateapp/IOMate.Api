using Microsoft.EntityFrameworkCore;
using IOMate.Domain.Entities;
using IOMate.Infra.Context;
using IOMate.Infra.Repositories;

public class BaseRepositoryTests
{
    private class TestEntity : BaseEntity
    {
        public string? Name { get; set; }
    }

    private class TestDbContext : AppDbContext
    {
        public TestDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<TestEntity> TestEntities { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TestEntity>();
        }
    }

    private TestDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new TestDbContext(options);
    }

    [Fact]
    public async Task Add_ShouldSetDateCreated_AndPersist()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new BaseRepository<TestEntity>(context);
        var entity = new TestEntity { Name = "Test" };

        // Act
        repo.Add(entity);
        await context.SaveChangesAsync();

        // Assert
        var saved = context.Set<TestEntity>().FirstOrDefault();
        Assert.NotNull(saved);
        Assert.NotNull(saved!.DateCreated);
        Assert.Equal("Test", saved.Name);
    }

    [Fact]
    public async Task Update_ShouldSetDateModified()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new BaseRepository<TestEntity>(context);
        var entity = new TestEntity { Name = "Test" };
        context.Add(entity);
        await context.SaveChangesAsync();

        // Act
        entity.Name = "Updated";
        repo.Update(entity);
        await context.SaveChangesAsync();

        // Assert
        var updated = context.Set<TestEntity>().First();
        Assert.NotNull(updated.DateModified);
        Assert.Equal("Updated", updated.Name);
    }

    [Fact]
    public async Task Delete_ShouldSetDateDeleted()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new BaseRepository<TestEntity>(context);
        var entity = new TestEntity { Name = "Test" };
        context.Add(entity);
        await context.SaveChangesAsync();

        // Act
        repo.Delete(entity);
        await context.SaveChangesAsync();

        // Assert
        Assert.NotNull(entity.DateDeleted);
        Assert.DoesNotContain(entity, context.Set<TestEntity>());
    }

    [Fact]
    public async Task GetPagedAsync_ShouldReturnCorrectPage()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new BaseRepository<TestEntity>(context);
        for (int i = 1; i <= 20; i++)
        {
            context.Add(new TestEntity { Name = $"Entity{i}" });
            await context.SaveChangesAsync();
        }

        // Act
        var page = await repo.GetPagedAsync(2, 5, CancellationToken.None);

        // Assert
        Assert.Equal(5, page.Count);
        Assert.Equal("Entity6", page[0].Name);
    }

    [Fact]
    public async Task CountAsync_ShouldReturnTotal()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new BaseRepository<TestEntity>(context);
        context.Add(new TestEntity { Name = "A" });
        context.Add(new TestEntity { Name = "B" });
        await context.SaveChangesAsync();

        // Act
        var count = await repo.CountAsync(CancellationToken.None);

        // Assert
        Assert.Equal(2, count);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEntity()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new BaseRepository<TestEntity>(context);
        var entity = new TestEntity { Name = "Test" };
        context.Add(entity);
        await context.SaveChangesAsync();

        // Act
        var found = await repo.GetByIdAsync(entity.Id, CancellationToken.None);

        // Assert
        Assert.NotNull(found);
        Assert.Equal(entity.Id, found!.Id);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAll()
    {
        // Arrange
        using var context = CreateContext();
        var repo = new BaseRepository<TestEntity>(context);
        context.Add(new TestEntity { Name = "A" });
        context.Add(new TestEntity { Name = "B" });
        await context.SaveChangesAsync();

        // Act
        var all = await repo.GetAllAsync(CancellationToken.None);

        // Assert
        Assert.Equal(2, all!.Count);
    }
}