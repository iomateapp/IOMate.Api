using IOMate.Application.Security;
using IOMate.Domain.Entities;
using IOMate.Domain.Enums;
using IOMate.Domain.Interfaces;
using IOMate.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace IOMate.Infra.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext Context;
        protected readonly ICurrentUserContext CurrentUserContext;

        public BaseRepository(AppDbContext context, ICurrentUserContext currentUserContext)
        {
            Context = context;
            CurrentUserContext = currentUserContext;
        }

        public void Add(T entity)
        {
            AddEventDynamic(entity, CurrentUserContext.User.Id, EventType.Created);
            Context.Add(entity);
        }

        public void Update(T entity)
        {
            AddEventDynamic(entity, CurrentUserContext.User.Id, EventType.Updated);
            Context.Update(entity);
        }

        public void Delete(T entity)
        {
            AddEventDynamic(entity, CurrentUserContext.User.Id, EventType.Deleted);
            Context.Remove(entity);
        }

        public async Task<List<T>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            return await Context.Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return await Context.Set<T>().CountAsync(cancellationToken);
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await Context.Set<T>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<List<T>?> GetAllAsync(CancellationToken cancellationToken)
        {
            return await Context.Set<T>().ToListAsync(cancellationToken);
        }

        private void AddEventDynamic(T entity, Guid ownerId, EventType type)
        {
            var eventType = typeof(EventEntity<>).MakeGenericType(typeof(T));
            var eventInstance = Activator.CreateInstance(eventType);

            eventType.GetProperty("Id")?.SetValue(eventInstance, Guid.NewGuid());
            eventType.GetProperty("OwnerId")?.SetValue(eventInstance, ownerId);
            eventType.GetProperty("Type")?.SetValue(eventInstance, type);
            eventType.GetProperty("Date")?.SetValue(eventInstance, DateTimeOffset.UtcNow);
            eventType.GetProperty("EntityId")?.SetValue(eventInstance, entity.Id);
            eventType.GetProperty("Entity")?.SetValue(eventInstance, entity);

            var eventsProp = typeof(T).GetProperty("Events");
            var eventsList = eventsProp?.GetValue(entity) as System.Collections.IList;
            eventsList?.Add(eventInstance);
        }
    }
}
