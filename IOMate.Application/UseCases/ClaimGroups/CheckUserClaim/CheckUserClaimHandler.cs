using IOMate.Domain.Interfaces;
using MediatR;

namespace IOMate.Application.UseCases.ClaimGroups.CheckUserClaim
{
    public class CheckUserClaimHandler : IRequestHandler<CheckUserClaimQuery, bool>
    {
        private readonly IClaimGroupRepository _claimGroupRepository;

        public CheckUserClaimHandler(IClaimGroupRepository claimGroupRepository)
        {
            _claimGroupRepository = claimGroupRepository;
        }

        public async Task<bool> Handle(CheckUserClaimQuery request, CancellationToken cancellationToken)
        {
            return await _claimGroupRepository.UserHasClaimAsync(
                request.UserId,
                request.Resource,
                request.Action,
                cancellationToken);
        }
    }
}