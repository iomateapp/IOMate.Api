using AutoMapper;
using IOMate.Application.UseCases.Users.DeleteUser;
using IOMate.Domain.Entities;
using Xunit;

public class DeleteUserMapperTests
{
    private readonly IMapper _mapper;

    public DeleteUserMapperTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<DeleteUserMapper>();
        });
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Should_Map_User_To_DeleteUserResponseDto()
    {
        var user = new User { Id = Guid.NewGuid(), Email = "test@test.com" };
        var dto = _mapper.Map<DeleteUserResponseDto>(user);

        Assert.Equal(user.Id, dto.Id);
        Assert.Equal(user.Email, dto.Email);
    }
}