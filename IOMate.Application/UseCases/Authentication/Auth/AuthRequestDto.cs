using MediatR;

namespace IOMate.Application.UseCases.Authentication.Auth
{
    public sealed record AuthenticationRequestDto(string Email, string Password) : IRequest<AuthResponseDto>;
}
