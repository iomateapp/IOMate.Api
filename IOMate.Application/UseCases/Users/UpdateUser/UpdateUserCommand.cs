using MediatR;

namespace IOMate.Application.UseCases.Users.UpdateUser
{
    public sealed record UpdateUserCommand(Guid Id, UpdateUserRequestDto Request) : IRequest<UpdateUserResponseDto>;
}
