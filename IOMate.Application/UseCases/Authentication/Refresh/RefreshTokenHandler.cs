using IOMate.Application.Resources;
using IOMate.Application.Shared.Exceptions;
using IOMate.Application.UseCases.Authentication.Auth;
using IOMate.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IOMate.Application.UseCases.Authentication.Refresh
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenRequestDto, AuthResponseDto>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;
        private readonly IStringLocalizer<Messages> _stringLocalizer;

        public RefreshTokenHandler(IJwtTokenGenerator jwtTokenGenerator,
            IUserRepository userRepository, IStringLocalizer<Messages> stringLocalizer)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<AuthResponseDto> Handle(RefreshTokenRequestDto request, CancellationToken cancellationToken)
        {
            var principal = _jwtTokenGenerator.GetPrincipalFromExpiredToken(request.RefreshToken, isRefreshToken: true);
            if (principal == null)
                throw new UnauthorizedException(_stringLocalizer["InvalidRefreshToken"]);

            var userId = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                      ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new UnauthorizedException(_stringLocalizer["ResourceNotFound"]);

            var user = await _userRepository.GetByIdAsync(Guid.Parse(userId), cancellationToken);
            if (user == null)
                throw new UnauthorizedException(_stringLocalizer["ResourceNotFound"]);

            var newAccessToken = _jwtTokenGenerator.GenerateToken(user);
            var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken(user);

            return new AuthResponseDto
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}