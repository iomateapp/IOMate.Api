using AutoMapper;
using IOMate.Application.Shared.Exceptions;
using IOMate.Domain.Entities;
using IOMate.Domain.Interfaces;
using MediatR;

namespace IOMate.Application.UseCases.CreateUser
{
    internal class CreateUserHandler : IRequestHandler<CreateUserRequestDto, CreateUserResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateUserHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<CreateUserResponseDto> Handle(CreateUserRequestDto request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmail(request.Email, cancellationToken);
            if (existingUser != null)
                throw new BadRequestException($"O e-mail '{request.Email}' já está em uso.");

            var user = _mapper.Map<User>(request);
            _userRepository.Add(user);
            await _unitOfWork.Commit(cancellationToken);
            return _mapper.Map<CreateUserResponseDto>(user);
        }
    }
}
