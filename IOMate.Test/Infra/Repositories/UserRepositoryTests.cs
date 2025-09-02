using IOMate.Application.Security;
using IOMate.Domain.Entities;
using IOMate.Domain.Enums;
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

    [Fact]
    public async Task GetUserEventsWithOwnerAsync_ShouldReturnEventsWithOwners_WhenEventsExist()
    {
        // Arrange
        using var context = CreateContext();
        var currentUserContextMock = new Mock<ICurrentUserContext>();
        var repo = new UserRepository(context, currentUserContextMock.Object);

        var user = new User { Id = Guid.NewGuid(), Email = "test@email.com" };
        context.Users.Add(user);

        var owner = new User { Id = Guid.NewGuid(), FirstName = "Owner", LastName = "User" };
        context.Users.Add(owner);

        var event1 = new EventEntity<User>
        {
            Id = Guid.NewGuid(),
            EntityId = user.Id,
            Type = EventType.Created,
            Date = DateTimeOffset.UtcNow,
            OwnerId = owner.Id,
            Owner = owner
        };

        var event2 = new EventEntity<User>
        {
            Id = Guid.NewGuid(),
            EntityId = user.Id,
            Type = EventType.Updated,
            Date = DateTimeOffset.UtcNow.AddMinutes(-10),
            OwnerId = owner.Id,
            Owner = owner
        };

        context.UserEvents.AddRange(event1, event2);
        await context.SaveChangesAsync();

        // Act
        var events = await repo.GetUserEventsWithOwnerAsync(user.Id, CancellationToken.None);

        // Assert
        Assert.NotNull(events);
        Assert.Equal(2, events.Count);
        Assert.Equal(event1.Id, events[0].Id); // Ordered by Date descending
        Assert.Equal(event2.Id, events[1].Id);
        Assert.All(events, e => Assert.NotNull(e.Owner));
        Assert.All(events, e => Assert.Equal(owner.Id, e.Owner!.Id));
    }

    [Fact]
    public async Task GetUserEventsWithOwnerAsync_ShouldReturnEmptyList_WhenNoEventsExist()
    {
        // Arrange
        using var context = CreateContext();
        var currentUserContextMock = new Mock<ICurrentUserContext>();
        var repo = new UserRepository(context, currentUserContextMock.Object);

        var user = new User { Id = Guid.NewGuid(), Email = "test@email.com" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Act
        var events = await repo.GetUserEventsWithOwnerAsync(user.Id, CancellationToken.None);

        // Assert
        Assert.NotNull(events);
        Assert.Empty(events);
    }

    [Fact]
    public async Task GetUserEventsWithOwnerAsync_ShouldThrowException_WhenContextFails()
    {
        // Arrange
        using var context = CreateContext();
        var currentUserContextMock = new Mock<ICurrentUserContext>();
        var repo = new UserRepository(context, currentUserContextMock.Object);

        var userId = Guid.NewGuid();

        // Simulate a failure in the database context
        context.Database.EnsureDeleted();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            repo.GetUserEventsWithOwnerAsync(userId, CancellationToken.None));
    }
}