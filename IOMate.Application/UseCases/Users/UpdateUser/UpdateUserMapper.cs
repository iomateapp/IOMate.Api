using AutoMapper;
using IOMate.Domain.Entities;

namespace IOMate.Application.UseCases.Users.UpdateUser
{
    public sealed class UpdateUserMapper : Profile
    {
        public UpdateUserMapper()
        {
            CreateMap<UpdateUserRequestDto, User>();
            CreateMap<User, UpdateUserResponseDto>();
        }
    }
}
