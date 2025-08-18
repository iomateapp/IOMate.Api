using MediatR;

namespace IOMate.Application.UseCases.Users.DeleteUser
{
    public sealed record DeleteUserRequestDto(Guid Id) : IRequest<DeleteUserResponseDto>;
}
