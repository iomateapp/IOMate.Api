using IOMate.Api.Domain.Entities;

namespace IOMate.Api.Application.Users.Queries.GetUsers;

public class UpdateUserDto
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}
