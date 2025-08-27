using IOMate.Application.UseCases.Authentication.Auth;
using MediatR;

namespace IOMate.Application.UseCases.Authentication.Refresh
{
    public sealed record RefreshTokenRequestDto : IRequest<AuthResponseDto>
    {
        public string? RefreshToken { get; set; }
    }
}