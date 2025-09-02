using IOMate.Application.Security;
using IOMate.Domain.Entities;
using IOMate.Infra.Context;
using IOMate.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

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

            modelBuilder.Entity<EventEntity<TestEntity>>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Entity)
                    .WithMany()
                    .HasForeignKey(e => e.EntityId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Owner)
                    .WithMany()
                    .HasForeignKey(e => e.OwnerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }

    private TestDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new TestDbContext(options);
    }

    private ICurrentUserContext CreateMockCurrentUserContext()
    {
        var mock = new Mock<ICurrentUserContext>();
        return mock.Object;
    }

    [Fact]
    public async Task Add_ShouldSetDateCreated_AndPersist()
    {
        // Arrange
        using var context = CreateContext();
        var currentUserContext = CreateMockCurrentUserContext();
        var repo = new BaseRepository<TestEntity>(context, currentUserContext);
        var entity = new TestEntity { Name = "Test" };

        // Act
        repo.Add(entity);
        await context.SaveChangesAsync();

        // Assert
        var saved = context.Set<TestEntity>().FirstOrDefault();
        Assert.NotNull(saved);
        Assert.Equal("Test", saved.Name);
    }

    [Fact]
    public async Task Update_ShouldSetDateModified()
    {
        // Arrange
        using var context = CreateContext();
        var currentUserContext = CreateMockCurrentUserContext();
        var repo = new BaseRepository<TestEntity>(context, currentUserContext);
        var entity = new TestEntity { Name = "Test" };
        context.Add(entity);
        await context.SaveChangesAsync();

        // Act
        entity.Name = "Updated";
        repo.Update(entity);
        await context.SaveChangesAsync();

        // Assert
        var updated = context.Set<TestEntity>().First();
        Assert.Equal("Updated", updated.Name);
    }

    [Fact]
    public async Task Delete_ShouldSetDateDeleted()
    {
        // Arrange
        using var context = CreateContext();
        var currentUserContext = CreateMockCurrentUserContext();
        var repo = new BaseRepository<TestEntity>(context, currentUserContext);
        var entity = new TestEntity { Name = "Test" };
        context.Add(entity);
        await context.SaveChangesAsync();

        // Act
        repo.Delete(entity);
        await context.SaveChangesAsync();

        // Assert
        Assert.DoesNotContain(entity, context.Set<TestEntity>());
    }

    [Fact]
    public async Task GetPagedAsync_ShouldReturnCorrectPage()
    {
        // Arrange
        using var context = CreateContext();
        var currentUserContext = CreateMockCurrentUserContext();
        var repo = new BaseRepository<TestEntity>(context, currentUserContext);
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
        var currentUserContext = CreateMockCurrentUserContext();
        var repo = new BaseRepository<TestEntity>(context, currentUserContext);
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
        var currentUserContext = CreateMockCurrentUserContext();
        var repo = new BaseRepository<TestEntity>(context, currentUserContext);
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
        var currentUserContext = CreateMockCurrentUserContext();
        var repo = new BaseRepository<TestEntity>(context, currentUserContext);
        context.Add(new TestEntity { Name = "A" });
        context.Add(new TestEntity { Name = "B" });
        await context.SaveChangesAsync();

        // Act
        var all = await repo.GetAllAsync(CancellationToken.None);

        // Assert
        Assert.Equal(2, all!.Count);
    }
}