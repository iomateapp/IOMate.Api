using AutoMapper;
using IOMate.Domain.Interfaces;
using MediatR;

namespace IOMate.Application.UseCases.ClaimGroups.GetUserGroups
{
    public class GetUserClaimGroupsHandler : IRequestHandler<GetUserClaimGroupsQuery, List<UserClaimGroupResponseDto>>
    {
        private readonly IClaimGroupRepository _claimGroupRepository;
        private readonly IMapper _mapper;

        public GetUserClaimGroupsHandler(IClaimGroupRepository claimGroupRepository, IMapper mapper)
        {
            _claimGroupRepository = claimGroupRepository;
            _mapper = mapper;
        }

        public async Task<List<UserClaimGroupResponseDto>> Handle(GetUserClaimGroupsQuery request, CancellationToken cancellationToken)
        {
            var claimGroups = await _claimGroupRepository.GetUserClaimGroupsAsync(request.UserId, cancellationToken);
            return _mapper.Map<List<UserClaimGroupResponseDto>>(claimGroups);
        }
    }
}