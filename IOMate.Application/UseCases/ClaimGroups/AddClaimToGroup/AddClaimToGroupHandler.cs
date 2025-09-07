using IOMate.Domain.Entities;
using IOMate.Domain.Interfaces;
using MediatR;

namespace IOMate.Application.UseCases.ClaimGroups.AddClaimToGroup
{
    public class AddClaimToGroupHandler : IRequestHandler<AddClaimToGroupCommand, bool>
    {
        private readonly IClaimGroupRepository _claimGroupRepository;
        private readonly IBaseRepository<ResourceClaim> _resourceClaimRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddClaimToGroupHandler(
            IClaimGroupRepository claimGroupRepository,
            IBaseRepository<ResourceClaim> resourceClaimRepository,
            IUnitOfWork unitOfWork)
        {
            _claimGroupRepository = claimGroupRepository;
            _resourceClaimRepository = resourceClaimRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(AddClaimToGroupCommand request, CancellationToken cancellationToken)
        {
            var claimGroup = await _claimGroupRepository.GetByIdAsync(request.ClaimGroupId, cancellationToken);
            if (claimGroup == null)
                return false;

            var claim = new ResourceClaim
            {
                Resource = request.Resource,
                Action = request.Action,
                ClaimGroupId = request.ClaimGroupId
            };

            _resourceClaimRepository.Add(claim);
            await _unitOfWork.Commit(cancellationToken);
            return true;
        }
    }
}