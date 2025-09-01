using AutoMapper;
using IOMate.Domain.Interfaces;
using MediatR;

namespace IOMate.Application.UseCases.Users.GetUserEvents
{
    public class GetUserEventsHandler : IRequestHandler<GetUserEventsRequestDto, List<GetUserEventResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public GetUserEventsHandler(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<List<GetUserEventResponseDto>> Handle(GetUserEventsRequestDto request, CancellationToken cancellationToken)
        {
            var events = await _userRepository.GetEntityEventsAsync(request.UserId, cancellationToken);

            return _mapper.Map<List<GetUserEventResponseDto>>(events);
        }
    }
}