namespace IOMate.Application.UseCases.ClaimGroups.GetUserGroups
{
    public class UserClaimGroupResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<ResourceClaimDto> Claims { get; set; } = new();
    }

    public class ResourceClaimDto
    {
        public Guid Id { get; set; }
        public string Resource { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
    }
}