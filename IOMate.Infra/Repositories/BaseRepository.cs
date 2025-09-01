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
            var eventType = typeof(EventEntity<>).MakeGenericType(typeof(T));

            var dbSetProp = Context.GetType().GetProperties()
                .FirstOrDefault(p =>
                    p.PropertyType.IsGenericType &&
                    p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                    p.PropertyType.GenericTypeArguments[0] == eventType);

            if (dbSetProp == null)
                return new List<object>();

            var dbSet = dbSetProp.GetValue(Context);

            var parameter = System.Linq.Expressions.Expression.Parameter(eventType, "e");
            var property = eventType.GetProperty("EntityId");
            var propertyAccess = System.Linq.Expressions.Expression.Property(parameter, property!);
            var equals = System.Linq.Expressions.Expression.Equal(
                propertyAccess,
                System.Linq.Expressions.Expression.Constant(entityId)
            );
            var lambda = System.Linq.Expressions.Expression.Lambda(equals, parameter);

            var whereMethod = typeof(Queryable).GetMethods()
                .First(m => m.Name == "Where" && m.GetParameters().Length == 2)
                .MakeGenericMethod(eventType);

            var query = whereMethod.Invoke(null, new object[] { dbSet, lambda });

            var dateProperty = eventType.GetProperty("Date");
            var datePropertyAccess = System.Linq.Expressions.Expression.Property(parameter, dateProperty!);
            var dateLambda = System.Linq.Expressions.Expression.Lambda(datePropertyAccess, parameter);

            var orderByDescMethod = typeof(Queryable).GetMethods()
                .First(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2)
                .MakeGenericMethod(eventType, typeof(DateTimeOffset));

            query = orderByDescMethod.Invoke(null, new object[] { query, dateLambda });

            var toListAsyncMethod = typeof(EntityFrameworkQueryableExtensions)
                .GetMethods()
                .First(m => m.Name == "ToListAsync" && m.GetParameters().Length == 2)
                .MakeGenericMethod(eventType);

            var task = (Task)toListAsyncMethod.Invoke(null, new object[] { query, cancellationToken })!;
            await task.ConfigureAwait(false);

            var resultProp = task.GetType().GetProperty("Result");
            var result = resultProp?.GetValue(task) as System.Collections.IEnumerable;

            return result?.Cast<object>().ToList() ?? new List<object>();
        }

        private void AddEvent(T entity, EventType type)
        {
            var eventsProp = typeof(T).GetProperty("Events");
            if (eventsProp == null) return;

            var eventType = typeof(EventEntity<>).MakeGenericType(typeof(T));
            var eventInstance = Activator.CreateInstance(eventType);
            if (eventInstance == null) return;

            eventType.GetProperty("Id")?.SetValue(eventInstance, Guid.NewGuid());
            eventType.GetProperty("OwnerId")?.SetValue(eventInstance, CurrentUserContext.User?.Id);
            eventType.GetProperty("Owner")?.SetValue(eventInstance, CurrentUserContext.User);
            eventType.GetProperty("Type")?.SetValue(eventInstance, type);
            eventType.GetProperty("Date")?.SetValue(eventInstance, DateTimeOffset.UtcNow);
            eventType.GetProperty("EntityId")?.SetValue(eventInstance, entity.Id);
            eventType.GetProperty("Entity")?.SetValue(eventInstance, entity);

            var eventsList = eventsProp.GetValue(entity) as System.Collections.IList;
            eventsList?.Add(eventInstance);

            var dbSet = Context.GetType().GetProperties()
                .FirstOrDefault(p =>
                    p.PropertyType.IsGenericType &&
                    p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                    p.PropertyType.GenericTypeArguments[0] == eventType);

            if (dbSet != null)
            {
                var set = dbSet.GetValue(Context);
                var addMethod = set?.GetType().GetMethod("Add");
                addMethod?.Invoke(set, new[] { eventInstance });
            }
        }
    }
}