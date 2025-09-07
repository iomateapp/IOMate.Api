namespace IOMate.Domain.Entities
{
    public class ClaimGroup : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<ResourceClaim> Claims { get; set; } = new();
        public List<UserClaimGroup> UserClaimGroups { get; set; } = new();
    }
}