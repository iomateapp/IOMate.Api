using AutoMapper;
using IOMate.Application.UseCases.Events.GetEvents;
using IOMate.Domain.Entities;
using IOMate.Domain.Enums;

public class GetEventsMapperTests
{
    private readonly IMapper _mapper;

    public GetEventsMapperTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<GetEventsMapper>();
        });
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Should_Map_EventEntity_To_GetEventResponseDto()
    {
        // Arrange
        var eventEntity = new EventEntity<User>
        {
            Id = Guid.NewGuid(),
            Type = EventType.Created,
            Date = DateTimeOffset.UtcNow,
            OwnerId = Guid.NewGuid(),
            EntityId = Guid.NewGuid(),
            Entity = new User { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" }
        };

        // Act
        var dto = _mapper.Map<GetEventResponseDto>(eventEntity);

        // Assert
        Assert.Equal(eventEntity.Id, dto.Id);
        Assert.Equal(eventEntity.Type, dto.Type);
        Assert.Equal(eventEntity.Date, dto.Date);
    }

    [Fact]
    public void Should_Map_User_To_UserOwnerDto()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com"
        };

        // Act
        var dto = _mapper.Map<UserOwnerDto>(user);

        // Assert
        Assert.Equal(user.Id, dto.Id);
        Assert.Equal(user.FirstName, dto.FirstName);
        Assert.Equal(user.LastName, dto.LastName);
        Assert.Equal(user.Email, dto.Email);
    }
}