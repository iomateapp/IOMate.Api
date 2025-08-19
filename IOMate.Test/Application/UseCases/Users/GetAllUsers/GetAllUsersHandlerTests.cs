using IOMate.Application.UseCases.Users.GetAllUsers;
using IOMate.Domain.Entities;
using IOMate.Domain.Interfaces;
using Moq;

public class GetAllUsersHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<AutoMapper.IMapper> _mapperMock = new();

    [Fact]
    public async Task Handle_ShouldReturnPagedResponse()
    {
        // Arrange
        var users = new List<User> { new(), new() };
        var usersDto = new List<GetAllUsersResponseDto> { new(), new() };

        _userRepositoryMock.Setup(r => r.GetPagedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);
        _userRepositoryMock.Setup(r => r.CountAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(2);
        _mapperMock.Setup(m => m.Map<List<GetAllUsersResponseDto>>(users))
            .Returns(usersDto);

        var handler = new GetAllUsersHandler(_userRepositoryMock.Object, _mapperMock.Object);
        var request = new GetAllUsersRequestDto(1, 10);

        // Act
        var result = await handler.Handle(request, default);

        // Assert
        Assert.Equal(2, result.TotalCount);
        Assert.Equal(2, result.Results.Count);
    }
}