using AutoMapper;
using IOMate.Application.Shared.Dtos;
using IOMate.Domain.Interfaces;
using MediatR;

namespace IOMate.Application.UseCases.Users.GetUserEvents
{
    public class GetUserEventsHandler : IRequestHandler<GetUserEventsRequestDto, List<GetEventResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserEventsHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<GetEventResponseDto>> Handle(GetUserEventsRequestDto request, CancellationToken cancellationToken)
        {
            var events = await _userRepository.GetUserEventsWithOwnerAsync(request.UserId, cancellationToken);

            // Carregar os owners manualmente (caso n�o tenha navega��o)
            var ownerIds = events.Select(e => e.OwnerId).Distinct().ToList();
            var owners = await _userRepository.GetOwnersByIdsAsync(ownerIds, cancellationToken); // Implemente esse m�todo no reposit�rio

            var ownerDict = owners.ToDictionary(u => u.Id);

            var dtos = events.Select(e =>
            {
                var dto = _mapper.Map<GetEventResponseDto>(e);
                dto.Owner = ownerDict.TryGetValue(e.OwnerId, out var owner)
                    ? _mapper.Map<UserOwnerDto>(owner)
                    : null;
                return dto;
            }).ToList();

            return dtos;
        }
    }
}