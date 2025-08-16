using AutoMapper;
using IOMate.Domain.Entities;

namespace IOMate.Application.UseCases.DeleteUser
{
    public sealed class DeleteUserMapper : Profile
    {
        public DeleteUserMapper()
        {
            CreateMap<DeleteUserRequestDto, User>();
            CreateMap<User, DeleteUserResponseDto>();
        }
    }
}
