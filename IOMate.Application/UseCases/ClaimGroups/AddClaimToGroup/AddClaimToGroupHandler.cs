using FluentValidation;
using IOMate.Application.Resources;
using IOMate.Domain.Entities;
using IOMate.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

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
            IUnitOfWork unitOfWork,
            IValidator<AddClaimToGroupCommand> validator,
            IStringLocalizer<Messages> stringLocalizer)
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

            var existingClaims = await _resourceClaimRepository.GetAllAsync(cancellationToken);
            var hasClaim = existingClaims?.Any(c =>
                c.ClaimGroupId == request.ClaimGroupId &&
                c.Resource == request.Resource &&
                c.Action == request.Action) ?? false;

            if (hasClaim)
                return true;

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