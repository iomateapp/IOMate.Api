using MediatR;

namespace IOMate.Application.UseCases.Authentication
{
    public sealed record AuthenticationRequestDto(string Email, string Password) : IRequest<AuthenticationResponseDto>;
}
