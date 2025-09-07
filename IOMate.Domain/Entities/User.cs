namespace IOMate.Domain.Entities
{
    public sealed class User : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public List<EventEntity<User>> Events { get; set; } = new();
        public List<UserClaimGroup> UserClaimGroups { get; set; } = new();
    }
}
