using AutoMapper;
using IOMate.Domain.Entities;

namespace IOMate.Application.UseCases.Users.GetUserById
{
    public sealed class GetUserByIdMapper : Profile
    {
        public GetUserByIdMapper()
        {
            CreateMap<GetUserByIdResponseDto, User>();
            CreateMap<User, GetUserByIdResponseDto>();
        }
    }
}
