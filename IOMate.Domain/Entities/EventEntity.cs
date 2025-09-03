using IOMate.Domain.Enums;

namespace IOMate.Domain.Entities
{
    public class EventEntity<TEntity> where TEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public EventType Type { get; set; }
        public DateTimeOffset Date { get; set; }

        public Guid EntityId { get; set; }
        public TEntity Entity { get; set; } = null!;

        public Guid OwnerId { get; set; }
        public User Owner { get; set; } = null!;
    }
}