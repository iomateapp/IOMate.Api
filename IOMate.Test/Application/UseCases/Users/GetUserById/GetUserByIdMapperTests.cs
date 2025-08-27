using AutoMapper;
using IOMate.Application.UseCases.Users.GetUserById;
using IOMate.Domain.Entities;

public class GetUserByIdMapperTests
{
    private readonly IMapper _mapper;

    public GetUserByIdMapperTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<GetUserByIdMapper>();
        });
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Should_Map_User_To_GetAllUsersResponseDto()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "user@email.com",
            FirstName = "First",
            LastName = "Last"
        };

        // Act
        var dto = _mapper.Map<GetUserByIdResponseDto>(user);

        // Assert
        Assert.Equal(user.Id, dto.Id);
        Assert.Equal(user.Email, dto.Email);
        Assert.Equal(user.FirstName, dto.FirstName);
        Assert.Equal(user.LastName, dto.LastName);
    }
}