using AutoMapper;
using IOMate.Application.UseCases.Users.GetUserEvents;
using IOMate.Domain.Entities;

namespace IOMate.Application.UseCases.Users.GetUserById
{
    public sealed class GetUserEventsMapper : Profile
    {
        public GetUserEventsMapper()
        {
            CreateMap<EventEntity<User>, GetUserEventResponseDto>();
            CreateMap<GetUserEventResponseDto, EventEntity<User>>();
        }
    }
}
