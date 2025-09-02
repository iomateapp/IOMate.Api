using AutoMapper;
using IOMate.Domain.Entities;

namespace IOMate.Application.UseCases.Events.GetEvents
{
    public sealed class GetEventsMapper : Profile
    {
        public GetEventsMapper()
        {
            CreateMap<EventEntity<User>, GetEventResponseDto>();
            CreateMap<User, UserOwnerDto>();
        }
    }
}
