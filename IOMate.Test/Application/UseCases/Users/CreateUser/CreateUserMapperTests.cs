using AutoMapper;
using IOMate.Application.UseCases.Users.CreateUser;
using IOMate.Domain.Entities;
public class CreateUserMapperTests
{
    private readonly IMapper _mapper;

    public CreateUserMapperTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CreateUserMapper>();
        });
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Should_Map_CreateUserRequestDto_To_User()
    {
        // Arrange
        var dto = new CreateUserRequestDto("email@test.com", "First", "Last", "password");

        // Act
        var user = _mapper.Map<User>(dto);

        // Assert
        Assert.Equal(dto.Email, user.Email);
        Assert.Equal(dto.FirstName, user.FirstName);
        Assert.Equal(dto.LastName, user.LastName);
    }

    [Fact]
    public void Should_Map_User_To_CreateUserResponseDto()
    {
        // Arrange
        var user = new User
        {
            Email = "email@test.com",
            FirstName = "First",
            LastName = "Last",
            Password = "hashedPassword"
        };

        // Act
        var dto = _mapper.Map<CreateUserResponseDto>(user);

        // Assert
        Assert.Equal(dto.Email, user.Email);
        Assert.Equal(dto.FirstName, user.FirstName);
        Assert.Equal(dto.LastName, user.LastName);
    }
}