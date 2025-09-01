using IOMate.Application.Shared.Dtos;
using MediatR;

namespace IOMate.Application.UseCases.Users.GetUserEvents
{
    public sealed record GetUserEventsRequestDto(Guid UserId) : IRequest<List<GetEventResponseDto>>;
}