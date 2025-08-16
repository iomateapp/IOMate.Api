using AutoMapper;
using IOMate.Domain.Interfaces;
using MediatR;
using IOMate.Application.Shared.Exceptions;
    
namespace IOMate.Application.UseCases.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdateUserResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateUserHandler(IUnitOfWork unitOfWork,
                                 IUserRepository userRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<UpdateUserResponseDto> Handle(UpdateUserCommand command,
                                                     CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(command.Id, cancellationToken);

            if (user is null) return default;

            var existingUser = await _userRepository.GetByEmail(command.Request.Email, cancellationToken);
            if (existingUser != null && existingUser.Id != user.Id)
                throw new BadRequestException($"O e-mail '{command.Request.Email}' já está em uso.");

            user.FirstName = command.Request.FirstName;
            user.LastName = command.Request.LastName;
            user.Email = command.Request.Email;

            _userRepository.Update(user);

            await _unitOfWork.Commit(cancellationToken);

            return _mapper.Map<UpdateUserResponseDto>(user);
        }
    }
}
