using IOMate.Domain.Enums;

namespace IOMate.Application.Shared.Dtos
{
    public sealed record GetEventResponseDto
    {
        public Guid Id { get; set; }
        public UserOwnerDto? Owner { get; set; }
        public EventType Type { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}