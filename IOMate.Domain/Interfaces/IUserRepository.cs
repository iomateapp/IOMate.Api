using IOMate.Domain.Entities;

namespace IOMate.Domain.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByEmail(string email, CancellationToken cancellationToken);
        Task<List<EventEntity<User>>> GetUserEventsWithOwnerAsync(Guid userId, CancellationToken cancellationToken);
        Task<List<User>> GetOwnersByIdsAsync(IEnumerable<Guid> ownerIds, CancellationToken cancellationToken);
    }
}
