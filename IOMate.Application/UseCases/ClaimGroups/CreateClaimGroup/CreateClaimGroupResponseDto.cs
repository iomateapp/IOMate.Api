namespace IOMate.Application.UseCases.ClaimGroups.CreateClaimGroup
{
    public class CreateClaimGroupResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}