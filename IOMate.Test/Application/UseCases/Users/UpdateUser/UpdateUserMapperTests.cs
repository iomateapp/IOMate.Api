using AutoMapper;
using IOMate.Application.UseCases.Users.UpdateUser;
using IOMate.Domain.Entities;
using Xunit;

public class UpdateUserMapperTests
{
    private readonly IMapper _mapper;

    public UpdateUserMapperTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UpdateUserMapper>();
        });
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Should_Map_User_To_UpdateUserResponseDto()
    {
        var user = new User { Id = Guid.NewGuid(), Email = "test@test.com", FirstName = "First", LastName = "Last" };
        var dto = _mapper.Map<UpdateUserResponseDto>(user);

        Assert.Equal(user.Id, dto.Id);
        Assert.Equal(user.Email, dto.Email);
        Assert.Equal(user.FirstName, dto.FirstName);
        Assert.Equal(user.LastName, dto.LastName);
    }
}