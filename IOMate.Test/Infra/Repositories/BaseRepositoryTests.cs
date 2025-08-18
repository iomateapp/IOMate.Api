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
        using var context = CreateContext();
        var repo = new BaseRepository<TestEntity>(context);
        var entity = new TestEntity { Name = "Test" };

        repo.Add(entity);
        await context.SaveChangesAsync();

        var saved = context.Set<TestEntity>().FirstOrDefault();
        Assert.NotNull(saved);
        Assert.NotNull(saved!.DateCreated);
        Assert.Equal("Test", saved.Name);
    }

    [Fact]
    public async Task Update_ShouldSetDateModified()
    {
        using var context = CreateContext();
        var repo = new BaseRepository<TestEntity>(context);
        var entity = new TestEntity { Name = "Test" };
        context.Add(entity);
        await context.SaveChangesAsync();

        entity.Name = "Updated";
        repo.Update(entity);
        await context.SaveChangesAsync();

        var updated = context.Set<TestEntity>().First();
        Assert.NotNull(updated.DateModified);
        Assert.Equal("Updated", updated.Name);
    }

    [Fact]
    public async Task Delete_ShouldSetDateDeleted()
    {
        using var context = CreateContext();
        var repo = new BaseRepository<TestEntity>(context);
        var entity = new TestEntity { Name = "Test" };
        context.Add(entity);
        await context.SaveChangesAsync();

        repo.Delete(entity);
        await context.SaveChangesAsync();

        Assert.NotNull(entity.DateDeleted);
        Assert.DoesNotContain(entity, context.Set<TestEntity>());
    }

    [Fact]
    public async Task GetPagedAsync_ShouldReturnCorrectPage()
    {
        using var context = CreateContext();
        var repo = new BaseRepository<TestEntity>(context);
        for (int i = 1; i <= 20; i++)
        {
            context.Add(new TestEntity { Name = $"Entity{i}" });
            await context.SaveChangesAsync();
        }

        var page = await repo.GetPagedAsync(2, 5, CancellationToken.None);

        Assert.Equal(5, page.Count);
        Assert.Equal("Entity6", page[0].Name);
    }

    [Fact]
    public async Task CountAsync_ShouldReturnTotal()
    {
        using var context = CreateContext();
        var repo = new BaseRepository<TestEntity>(context);
        context.Add(new TestEntity { Name = "A" });
        context.Add(new TestEntity { Name = "B" });
        await context.SaveChangesAsync();

        var count = await repo.CountAsync(CancellationToken.None);
        Assert.Equal(2, count);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEntity()
    {
        using var context = CreateContext();
        var repo = new BaseRepository<TestEntity>(context);
        var entity = new TestEntity { Name = "Test" };
        context.Add(entity);
        await context.SaveChangesAsync();

        var found = await repo.GetByIdAsync(entity.Id, CancellationToken.None);
        Assert.NotNull(found);
        Assert.Equal(entity.Id, found!.Id);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAll()
    {
        using var context = CreateContext();
        var repo = new BaseRepository<TestEntity>(context);
        context.Add(new TestEntity { Name = "A" });
        context.Add(new TestEntity { Name = "B" });
        await context.SaveChangesAsync();

        var all = await repo.GetAllAsync(CancellationToken.None);
        Assert.Equal(2, all!.Count);
    }
}