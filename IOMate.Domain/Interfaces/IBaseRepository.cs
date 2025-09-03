using IOMate.Domain.Entities;

namespace IOMate.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<T>?> GetAllAsync(CancellationToken cancellationToken);
        Task<List<T>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<int> CountAsync(CancellationToken cancellationToken);
        Task<List<object>> GetEntityEventsAsync(Guid entityId, CancellationToken cancellationToken);
        Task<List<EventEntity<T>>> GetEntityEventsWithOwnerAsync(Guid entityId, CancellationToken cancellationToken);
        Task<List<User>> GetOwnersByIdsAsync(IEnumerable<Guid> ownerIds, CancellationToken cancellationToken);
    }
}
