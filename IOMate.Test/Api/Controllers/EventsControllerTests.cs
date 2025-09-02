using IOMate.Api.Controllers;
using IOMate.Application.UseCases.Events.GetEvents;
using IOMate.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

public class EventsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly EventsController _controller;

    public EventsControllerTests()
    {
        _controller = new EventsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetEvents_ReturnsOk_WithEvents()
    {
        // Arrange
        var entityId = Guid.NewGuid();
        var events = new List<GetEventResponseDto>
        {
            new() { Id = Guid.NewGuid(), Type = EventType.Created, Date = DateTimeOffset.UtcNow },
            new() { Id = Guid.NewGuid(), Type = EventType.Updated, Date = DateTimeOffset.UtcNow }
        };

        _mediatorMock.Setup(m => m.Send(It.Is<GetEventsRequestDto>(r => r.UserId == entityId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(events);

        // Act
        var result = await _controller.GetEvents(entityId, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(events, okResult.Value);
    }

    [Fact]
    public async Task GetEvents_ReturnsOk_WithEmptyList_WhenNoEvents()
    {
        // Arrange
        var entityId = Guid.NewGuid();
        var events = new List<GetEventResponseDto>();

        _mediatorMock.Setup(m => m.Send(It.Is<GetEventsRequestDto>(r => r.UserId == entityId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(events);

        // Act
        var result = await _controller.GetEvents(entityId, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Empty((List<GetEventResponseDto>)okResult.Value!);
    }

    [Fact]
    public async Task GetEvents_ThrowsException_WhenMediatorFails()
    {
        // Arrange
        var entityId = Guid.NewGuid();
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetEventsRequestDto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Unexpected error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _controller.GetEvents(entityId, CancellationToken.None));
        Assert.Equal("Unexpected error", exception.Message);
    }
}