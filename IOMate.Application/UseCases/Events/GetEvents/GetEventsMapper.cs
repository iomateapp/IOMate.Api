using AutoMapper;
using IOMate.Domain.Entities;

namespace IOMate.Application.UseCases.Events.GetEvents
{
    public sealed class GetEventsMapper : Profile
    {
        public GetEventsMapper()
        {
            CreateMap(typeof(EventEntity<>), typeof(GetEventResponseDto));
            CreateMap<User, UserOwnerDto>();
        }
    }
}
