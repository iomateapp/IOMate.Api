using AutoMapper;
using IOMate.Domain.Entities;

namespace IOMate.Application.UseCases.Users.GetAllUsers
{
    public sealed class GetAllUsersMapper : Profile
    {
        public GetAllUsersMapper()
        {
            CreateMap<User, GetAllUsersResponseDto>();
        }
    }
}
