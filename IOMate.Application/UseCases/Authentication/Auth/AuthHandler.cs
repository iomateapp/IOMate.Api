using AutoMapper;
using IOMate.Application.Resources;
using IOMate.Application.Shared.Exceptions;
using IOMate.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace IOMate.Application.UseCases.Authentication.Auth
{
    public class AuthHandler : IRequestHandler<AuthRequestDto, AuthResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IStringLocalizer<Messages> _stringLocalizer;

        public AuthHandler(
            IUserRepository userRepository,
            IMapper mapper,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            IStringLocalizer<Messages> stringLocalizer)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<AuthResponseDto> Handle(AuthRequestDto request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmail(request.Email, cancellationToken);

            if (existingUser == null)
                throw new BadRequestException(_stringLocalizer["InvalidCredentials"]);

            if (!_passwordHasher.VerifyPassword(request.Password, existingUser.Password!))
                throw new BadRequestException(_stringLocalizer["InvalidCredentials"]);

            var token = _jwtTokenGenerator.GenerateToken(existingUser);

            var response = new AuthResponseDto
            {
                Token = token
            };
            return response;
        }
    }
}