using IOMate.Domain.Entities;

namespace IOMate.Domain.Interfaces
{
    public interface IClaimGroupRepository : IBaseRepository<ClaimGroup>
    {
        Task<List<ClaimGroup>> GetUserClaimGroupsAsync(Guid userId, CancellationToken cancellationToken);
        Task<bool> UserHasClaimAsync(Guid userId, string resource, string action, CancellationToken cancellationToken);
    }
}