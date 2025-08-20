using AutoMapper;
using IOMate.Application.Shared.Exceptions;
using IOMate.Domain.Entities;
using IOMate.Domain.Interfaces;
using MediatR;

namespace IOMate.Application.UseCases.Authentication.Auth
{
    public class AuthHandler : IRequestHandler<AuthRequestDto, AuthResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthHandler(
            IUserRepository userRepository,
            IMapper mapper,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthResponseDto> Handle(AuthRequestDto request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmail(request.Email, cancellationToken);

            if (existingUser == null)
                throw new BadRequestException("Usuário ou senha inválidos.");

            if (!_passwordHasher.VerifyPassword(request.Password, existingUser.Password!))
                throw new BadRequestException("Usuário ou senha inválidos.");

            var token = _jwtTokenGenerator.GenerateToken(existingUser);

            var response = new AuthResponseDto 
            {
                Token = token
            };
            return response;
        }
    }
}