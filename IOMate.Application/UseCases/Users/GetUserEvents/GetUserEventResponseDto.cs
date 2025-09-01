using IOMate.Domain.Enums;

namespace IOMate.Application.UseCases.Users.GetUserEvents
{
    public sealed record GetUserEventResponseDto
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public EventType Type { get; set; }
        public DateTimeOffset Date { get; set; }
        public Guid EntityId { get; set; }
    }
}