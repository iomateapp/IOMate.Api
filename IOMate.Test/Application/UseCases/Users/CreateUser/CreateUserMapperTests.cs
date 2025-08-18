using AutoMapper;
using IOMate.Application.UseCases.Users.CreateUser;
using IOMate.Domain.Entities;
using Xunit;

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
        var dto = new CreateUserRequestDto("email@test.com", "First", "Last", "password");
        var user = _mapper.Map<User>(dto);

        Assert.Equal(dto.Email, user.Email);
        Assert.Equal(dto.FirstName, user.FirstName);
        Assert.Equal(dto.LastName, user.LastName);
    }

    [Fact]
    public void Should_Map_User_To_CreateUserResponseDto()
    {
        var user = new User
        {
            Email = "email@test.com",
            FirstName = "First",
            LastName = "Last",
            Password = "hashedPassword"
        };

        var dto = _mapper.Map<CreateUserResponseDto>(user);

        Assert.Equal(dto.Email, user.Email);
        Assert.Equal(dto.FirstName, user.FirstName);
        Assert.Equal(dto.LastName, user.LastName);
    }
}