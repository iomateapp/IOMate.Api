using IOMate.Application.UseCases.Users.GetUserById;
using IOMate.Domain.Entities;
using IOMate.Domain.Interfaces;
using Moq;

public class GetUserByIdHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<AutoMapper.IMapper> _mapperMock = new();

    [Fact]
    public async Task Handle_ShouldReturnUser_WhenUserExists()
    {
        var Id = Guid.NewGuid();
        var user = new User { Id = Id, Email = "test@email.com" };
        var responseDto = new GetUserByIdResponseDto { Id = Id, Email = "test@email.com" };

        _userRepositoryMock.Setup(r => r.GetByIdAsync(Id, default)).ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map<GetUserByIdResponseDto>(user)).Returns(responseDto);

        var handler = new GetUserByIdHandler(_userRepositoryMock.Object, _mapperMock.Object);
        var request = new GetUserByIdRequestDto(Id);

        var result = await handler.Handle(request, default);

        Assert.NotNull(result);
        Assert.Equal(Id, result.Id);
        Assert.Equal("test@email.com", result.Email);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenUserDoesNotExist()
    {
        var Id = Guid.NewGuid();
        _userRepositoryMock.Setup(r => r.GetByIdAsync(Id, default)).ReturnsAsync((User)null!);

        var handler = new GetUserByIdHandler(_userRepositoryMock.Object, _mapperMock.Object);
        var request = new GetUserByIdRequestDto(Id);

        var result = await handler.Handle(request, default);

        Assert.Null(result);
    }
}