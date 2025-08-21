using AutoMapper;
using IOMate.Application.Resources;
using IOMate.Application.Shared.Exceptions;
using IOMate.Domain.Entities;
using IOMate.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace IOMate.Application.UseCases.Users.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserRequestDto, CreateUserResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IStringLocalizer<Messages> _stringLocalizer;

        public CreateUserHandler(IUnitOfWork unitOfWork, IUserRepository userRepository,
            IMapper mapper, IPasswordHasher passwordHasher, IStringLocalizer<Messages> stringLocalizer)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<CreateUserResponseDto> Handle(CreateUserRequestDto request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmail(request.Email, cancellationToken);
            if (existingUser != null)
                throw new BadRequestException(_stringLocalizer["EmailAlreadyInUse", request.Email]);

            var user = _mapper.Map<User>(request);
            user.Password = _passwordHasher.HashPassword(request.Password);
            _userRepository.Add(user);
            await _unitOfWork.Commit(cancellationToken);
            return _mapper.Map<CreateUserResponseDto>(user);
        }
    }
}
