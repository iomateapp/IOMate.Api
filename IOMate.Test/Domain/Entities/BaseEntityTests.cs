using IOMate.Domain.Entities;

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
}