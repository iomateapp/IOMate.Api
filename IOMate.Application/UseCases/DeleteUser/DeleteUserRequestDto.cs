using MediatR;

namespace IOMate.Application.UseCases.CreateUser
{
    public sealed record DeleteUserRequestDto(Guid Id)  : IRequest<DeleteUserResponseDto>;
}
