using MediatR;

namespace IOMate.Application.UseCases.DeleteUser
{
    public sealed record DeleteUserRequestDto(Guid Id) : IRequest<DeleteUserResponseDto>;
}
