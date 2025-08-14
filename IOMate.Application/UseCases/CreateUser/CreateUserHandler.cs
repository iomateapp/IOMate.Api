using AutoMapper;
using IOMate.Domain.Interfaces;
using IOMate.Domain.Entities;
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
            var user = _mapper.Map<User>(request);
            _userRepository.Add(user);
            await _unitOfWork.Commit(cancellationToken);
            return _mapper.Map<CreateUserResponseDto>(user);
        }
    }
}
