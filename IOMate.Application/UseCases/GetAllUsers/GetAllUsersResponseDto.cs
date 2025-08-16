namespace IOMate.Application.UseCases.GetAllUsers
{
    public sealed record GetAllUsersResponseDto 
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
