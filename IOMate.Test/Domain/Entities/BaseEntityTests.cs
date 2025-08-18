using System;
using IOMate.Domain.Entities;
using Xunit;

public class BaseEntityTests
{
    private class TestEntity : BaseEntity { }

    [Fact]
    public void Should_Set_And_Get_Id()
    {
        var entity = new TestEntity();
        var id = Guid.NewGuid();
        entity.Id = id;
        Assert.Equal(id, entity.Id);
    }

    [Fact]
    public void Should_Set_And_Get_DateCreated()
    {
        var entity = new TestEntity();
        var now = DateTimeOffset.UtcNow;
        entity.DateCreated = now;
        Assert.Equal(now, entity.DateCreated);
    }

    [Fact]
    public void Should_Set_And_Get_DateModified()
    {
        var entity = new TestEntity();
        var now = DateTimeOffset.UtcNow;
        entity.DateModified = now;
        Assert.Equal(now, entity.DateModified);
    }

    [Fact]
    public void Should_Set_And_Get_DateDeleted()
    {
        var entity = new TestEntity();
        var now = DateTimeOffset.UtcNow;
        entity.DateDeleted = now;
        Assert.Equal(now, entity.DateDeleted);
    }
}