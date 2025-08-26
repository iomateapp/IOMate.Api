using MediatR;

namespace IOMate.Application.UseCases.Users.GetUserById
{
    public sealed record GetUserByIdRequestDto(Guid Id) : IRequest<GetUserByIdResponseDto>;
}