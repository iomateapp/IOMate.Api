using IOMate.Application.Security;
using IOMate.Domain.Entities;
using IOMate.Domain.Interfaces;
using IOMate.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace IOMate.Infra.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context, ICurrentUserContext currentUserContext)
            : base(context, currentUserContext) { }

        public async Task<User?> GetByEmail(string email, CancellationToken cancellationToken)
        {
            return await Context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<List<EventEntity<User>>> GetUserEventsWithOwnerAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await Context.UserEvents
                .Include(e => e.Owner)
                .Where(e => e.EntityId == userId)
                .OrderByDescending(e => e.Date)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<User>> GetOwnersByIdsAsync(IEnumerable<Guid> ownerIds, CancellationToken cancellationToken)
        {
            return await Context.Users
                .Where(u => ownerIds.Contains(u.Id))
                .ToListAsync(cancellationToken);
        }
    }
}
