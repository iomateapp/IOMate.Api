namespace IOMate.Application.UseCases.Users.CreateUser
{
    public sealed record CreateUserResponseDto
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
    }
}
