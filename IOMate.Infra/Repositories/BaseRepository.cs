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
            AddEvent(entity, EventType.Created);
            Context.Add(entity);
        }

        public void Update(T entity)
        {
            AddEvent(entity, EventType.Updated);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            AddEvent(entity, EventType.Deleted);
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

        public async Task<List<object>> GetEntityEventsAsync(Guid entityId, CancellationToken cancellationToken)
        {
            var events = await Context.Set<EventEntity<T>>()
                .Where(e => e.EntityId == entityId)
                .OrderByDescending(e => e.Date)
                .ToListAsync(cancellationToken);

            return events.Cast<object>().ToList();
        }

        public async Task<List<EventEntity<T>>> GetEntityEventsWithOwnerAsync(Guid entityId, CancellationToken cancellationToken)
        {
            return await Context.Set<EventEntity<T>>()
                .Include(e => e.Owner)
                .Where(e => e.EntityId == entityId)
                .OrderByDescending(e => e.Date)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<User>> GetOwnersByIdsAsync(IEnumerable<Guid> ownerIds, CancellationToken cancellationToken)
        {
            return await Context.Users
                .Where(u => ownerIds.Contains(u.Id))
                .ToListAsync(cancellationToken);
        }

        private void AddEvent(T entity, EventType type)
        {
            var eventInstance = new EventEntity<T>
            {
                Id = Guid.NewGuid(),
                OwnerId = CurrentUserContext.User?.Id ?? Guid.Empty,
                Type = type,
                Date = DateTimeOffset.UtcNow,
                EntityId = entity.Id,
                Entity = entity
            };

            var eventsProp = typeof(T).GetProperty("Events");
            if (eventsProp != null)
            {
                if (eventsProp.GetValue(entity) is IList<EventEntity<T>> eventsList)
                {
                    eventsList.Add(eventInstance);
                }
            }
            Context.Set<EventEntity<T>>().Add(eventInstance);
        }
    }
}