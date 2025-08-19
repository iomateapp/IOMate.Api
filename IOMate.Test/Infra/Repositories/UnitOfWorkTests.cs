using IOMate.Infra.Context;
using IOMate.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class UnitOfWorkTests
{
    [Fact]
    public async Task Commit_Should_Call_SaveChangesAsync()
    {
        // Arrange
        var dbContextMock = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
        dbContextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1)
            .Verifiable();

        var unitOfWork = new UnitOfWork(dbContextMock.Object);

        // Act
        await unitOfWork.Commit(CancellationToken.None);

        // Assert
        dbContextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}