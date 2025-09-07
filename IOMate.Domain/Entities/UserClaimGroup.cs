namespace IOMate.Domain.Entities
{
    public class UserClaimGroup : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid ClaimGroupId { get; set; }
        public ClaimGroup ClaimGroup { get; set; } = null!;
    }
}