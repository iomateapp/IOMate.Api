using MediatR;

namespace IOMate.Application.UseCases.Authentication.Auth
{
    public sealed record AuthRequestDto(string Email, string Password) : IRequest<AuthResponseDto>;
}
