using IOMate.Domain.Entities;
using IOMate.Domain.Interfaces;
using MediatR;

namespace IOMate.Application.UseCases.ClaimGroups.AssignToUser
{
    public class AssignClaimGroupToUserHandler : IRequestHandler<AssignClaimGroupToUserCommand, bool>
    {
        private readonly IClaimGroupRepository _claimGroupRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBaseRepository<UserClaimGroup> _userClaimGroupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignClaimGroupToUserHandler(
            IClaimGroupRepository claimGroupRepository,
            IUserRepository userRepository,
            IBaseRepository<UserClaimGroup> userClaimGroupRepository,
            IUnitOfWork unitOfWork)
        {
            _claimGroupRepository = claimGroupRepository;
            _userRepository = userRepository;
            _userClaimGroupRepository = userClaimGroupRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(AssignClaimGroupToUserCommand request, CancellationToken cancellationToken)
        {
            var claimGroup = await _claimGroupRepository.GetByIdAsync(request.ClaimGroupId, cancellationToken);
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (claimGroup == null || user == null)
                return false;

            var existingAssignments = await _userClaimGroupRepository.GetAllAsync(cancellationToken);
            var exists = existingAssignments?.Any(ucg =>
                ucg.UserId == request.UserId &&
                ucg.ClaimGroupId == request.ClaimGroupId) ?? false;

            if (exists)
                return true;

            var userClaimGroup = new UserClaimGroup
            {
                UserId = request.UserId,
                ClaimGroupId = request.ClaimGroupId
            };

            _userClaimGroupRepository.Add(userClaimGroup);
            await _unitOfWork.Commit(cancellationToken);
            return true;
        }
    }
}