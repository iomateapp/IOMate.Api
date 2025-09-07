using IOMate.Application.Security;
using IOMate.Domain.Entities;
using IOMate.Domain.Interfaces;
using IOMate.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace IOMate.Infra.Repositories
{
    public class ClaimGroupRepository : BaseRepository<ClaimGroup>, IClaimGroupRepository
    {
        public ClaimGroupRepository(AppDbContext context, ICurrentUserContext currentUserContext)
            : base(context, currentUserContext) { }

        public async Task<List<ClaimGroup>> GetUserClaimGroupsAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await Context.UserClaimGroups
                .Where(ucg => ucg.UserId == userId)
                .Include(ucg => ucg.ClaimGroup)
                .ThenInclude(cg => cg.Claims)
                .Select(ucg => ucg.ClaimGroup)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> UserHasClaimAsync(Guid userId, string resource, string action, CancellationToken cancellationToken)
        {
            return await Context.UserClaimGroups
                .Where(ucg => ucg.UserId == userId)
                .SelectMany(ucg => ucg.ClaimGroup.Claims)
                .AnyAsync(c => c.Resource == resource && c.Action == action, cancellationToken);
        }
    }
}