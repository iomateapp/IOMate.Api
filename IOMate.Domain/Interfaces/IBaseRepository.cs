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
    }
}
