using AutoMapper;
using IOMate.Application.UseCases.Events.GetEvents;
using IOMate.Domain.Entities;
using IOMate.Domain.Enums;
using IOMate.Domain.Interfaces;
using Moq;

namespace IOMate.Test.Application.UseCases.Events
{
    public class GetEventsHandlerTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly GetEventsHandler _handler;

        public GetEventsHandlerTest()
        {
            _handler = new GetEventsHandler(_userRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnEvents_WhenEventsExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var events = new List<EventEntity<User>>
            {
                new() { Id = Guid.NewGuid(), Type = EventType.Created, Date = DateTimeOffset.UtcNow, OwnerId = Guid.NewGuid() },
                new() { Id = Guid.NewGuid(), Type = EventType.Updated, Date = DateTimeOffset.UtcNow, OwnerId = Guid.NewGuid() }
            };
            var owners = new List<User>
            {
                new() { Id = events[0].OwnerId, FirstName = "John", LastName = "Doe" },
                new() { Id = events[1].OwnerId, FirstName = "Jane", LastName = "Smith" }
            };
            var eventDtos = new List<GetEventResponseDto>
            {
                new() { Id = events[0].Id, Type = events[0].Type, Date = events[0].Date },
                new() { Id = events[1].Id, Type = events[1].Type, Date = events[1].Date }
            };

            _userRepositoryMock.Setup(r => r.GetUserEventsWithOwnerAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(events);
            _userRepositoryMock.Setup(r => r.GetOwnersByIdsAsync(It.IsAny<IEnumerable<Guid>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(owners);
            _mapperMock.Setup(m => m.Map<GetEventResponseDto>(It.IsAny<EventEntity<User>>()))
                .Returns((EventEntity<User> e) => eventDtos.First(dto => dto.Id == e.Id));
            _mapperMock.Setup(m => m.Map<UserOwnerDto>(It.IsAny<User>()))
                .Returns((User u) => new UserOwnerDto { Id = u.Id, FirstName = u.FirstName, LastName = u.LastName });

            // Act
            var result = await _handler.Handle(new GetEventsRequestDto(userId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(eventDtos[0].Id, result[0].Id);
            Assert.Equal(eventDtos[1].Id, result[1].Id);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userRepositoryMock.Setup(r => r.GetUserEventsWithOwnerAsync(userId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Repository failure"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(new GetEventsRequestDto(userId), CancellationToken.None));
            Assert.Equal("Repository failure", exception.Message);
        }
    }
}