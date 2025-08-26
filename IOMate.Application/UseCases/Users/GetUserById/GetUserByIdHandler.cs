using AutoMapper;
using IOMate.Domain.Interfaces;
using MediatR;

namespace IOMate.Application.UseCases.Users.GetUserById
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdRequestDto, GetUserByIdResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserByIdHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<GetUserByIdResponseDto> Handle(GetUserByIdRequestDto request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
            if (user == null)
                return null!;
            return _mapper.Map<GetUserByIdResponseDto>(user);
        }
    }
}