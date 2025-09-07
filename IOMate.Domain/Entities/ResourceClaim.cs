namespace IOMate.Domain.Entities
{
    public class ResourceClaim : BaseEntity
    {
        public string Resource { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty; // read, write, delete, etc.
        public Guid ClaimGroupId { get; set; }
        public ClaimGroup ClaimGroup { get; set; } = null!;
    }
}