using AutoMapper;
using IOMate.Domain.Interfaces;
using MediatR;

namespace IOMate.Application.UseCases.GetAllUsers
{
    public sealed class GetAllUsersHandler : IRequestHandler<GetAllUsersRequestDto, List<GetAllUsersResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllUsersHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<GetAllUsersResponseDto>> Handle(GetAllUsersRequestDto request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<List<GetAllUsersResponseDto>>(users);
        }
    }
}
