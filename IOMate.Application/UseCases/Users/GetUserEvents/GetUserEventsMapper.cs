using AutoMapper;
using IOMate.Application.Shared.Dtos;
using IOMate.Domain.Entities;

namespace IOMate.Application.UseCases.Users.GetUserEvents
{
    public sealed class GetUserEventsMapper : Profile
    {
        public GetUserEventsMapper()
        {
            CreateMap<EventEntity<User>, GetEventResponseDto>();
            CreateMap<User, UserOwnerDto>();
        }
    }
}
