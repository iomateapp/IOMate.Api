using AutoMapper;
using IOMate.Domain.Entities;

namespace IOMate.Application.UseCases.CreateUser
{
    public sealed class CreateUserMapper : Profile
    {
        public CreateUserMapper() 
        {
            CreateMap<CreateUserRequestDto, User>();
            CreateMap<User, CreateUserResponseDto>();
        }
    }
}
