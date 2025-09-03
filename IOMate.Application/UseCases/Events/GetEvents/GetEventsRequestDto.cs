using MediatR;

namespace IOMate.Application.UseCases.Events.GetEvents
{
    public sealed record GetEventsRequestDto(Guid EntityId) : IRequest<List<GetEventResponseDto>>;
}