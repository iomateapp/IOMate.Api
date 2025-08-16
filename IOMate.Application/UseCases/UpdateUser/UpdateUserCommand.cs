using MediatR;

namespace IOMate.Application.UseCases.UpdateUser
{
    public sealed record UpdateUserCommand(Guid Id, UpdateUserRequestDto Request) : IRequest<UpdateUserResponseDto>;
}
