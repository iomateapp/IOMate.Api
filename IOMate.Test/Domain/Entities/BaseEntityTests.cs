using System;
using IOMate.Domain.Entities;
using Xunit;

public class BaseEntityTests
{
    private class TestEntity : BaseEntity { }

    [Fact]
    public void Should_Set_And_Get_Id()
    {
        // Arrange
        var entity = new TestEntity();
        var id = Guid.NewGuid();

        // Act
        entity.Id = id;

        // Assert
        Assert.Equal(id, entity.Id);
    }

    [Fact]
    public void Should_Set_And_Get_DateCreated()
    {
        // Arrange
        var entity = new TestEntity();
        var now = DateTimeOffset.UtcNow;

        // Act
        entity.DateCreated = now;

        // Assert
        Assert.Equal(now, entity.DateCreated);
    }

    [Fact]
    public void Should_Set_And_Get_DateModified()
    {
        // Arrange
        var entity = new TestEntity();
        var now = DateTimeOffset.UtcNow;

        // Act
        entity.DateModified = now;

        // Assert
        Assert.Equal(now, entity.DateModified);
    }

    [Fact]
    public void Should_Set_And_Get_DateDeleted()
    {
        // Arrange
        var entity = new TestEntity();
        var now = DateTimeOffset.UtcNow;

        // Act
        entity.DateDeleted = now;

        // Assert
        Assert.Equal(now, entity.DateDeleted);
    }
}