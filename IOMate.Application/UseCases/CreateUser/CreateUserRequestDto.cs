using MediatR;

namespace IOMate.Application.UseCases.CreateUser
{
    public sealed record CreateUserRequestDto(string Email, string FirstName, string LastName) : IRequest<CreateUserResponseDto>;
}
