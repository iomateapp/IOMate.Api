using MediatR;

namespace IOMate.Application.UseCases.UpdateUser
{
    public sealed record UpdateUserRequestDto(string FirstName, string LastName, string Email)
        : IRequest<UpdateUserResponseDto>;
}
