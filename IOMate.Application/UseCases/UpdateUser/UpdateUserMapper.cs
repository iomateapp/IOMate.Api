using AutoMapper;
using IOMate.Domain.Entities;

namespace IOMate.Application.UseCases.UpdateUser
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
