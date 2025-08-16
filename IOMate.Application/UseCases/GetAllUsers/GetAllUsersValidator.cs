using AutoMapper;
using FluentValidation;
using IOMate.Domain.Interfaces;
using MediatR;

namespace IOMate.Application.UseCases.GetAllUsers
{
    public class GetAllUsersValidator : AbstractValidator<GetAllUsersRequestDto>
    {
        public GetAllUsersValidator()
        {  
        }
    }
}
