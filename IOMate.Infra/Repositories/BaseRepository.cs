using IOMate.Domain.Entities;
using IOMate.Domain.Interfaces;
using IOMate.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace IOMate.Infra.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext Context;

        public BaseRepository(AppDbContext context)
        {
            Context = context;
        }

        public void Add(T entity) 
        {
            entity.DateCreated = DateTime.UtcNow;
            Context.Add(entity);
        }
        
        public void Update(T entity) 
        { 
            entity.DateModified = DateTime.UtcNow;
            Context.Update(entity);
        }

        public void Delete(T entity)
        { 
            entity.DateDeleted = DateTime.UtcNow;
            Context.Remove(entity);
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await Context.Set<T>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<List<T>?> GetAllAsync(CancellationToken cancellationToken)
        {
            return await Context.Set<T>().ToListAsync(cancellationToken);
        }
    }
}
