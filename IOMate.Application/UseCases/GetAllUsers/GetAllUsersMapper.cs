using AutoMapper;
using IOMate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOMate.Application.UseCases.GetAllUsers
{
    public sealed class GetAllUsersMapper : Profile
    {
        public GetAllUsersMapper()
        {
            CreateMap<User, GetAllUsersResponseDto>();
        }
    }
}
