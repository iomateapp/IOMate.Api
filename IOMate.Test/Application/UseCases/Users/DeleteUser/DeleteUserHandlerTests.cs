using AutoMapper;
using IOMate.Application.UseCases.Users.DeleteUser;
using IOMate.Domain.Entities;
using IOMate.Domain.Interfaces;
using Moq;

public class DeleteUserHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    [Fact]
    public async Task Handle_ShouldReturnDefault_WhenUserNotFound()
    {
        // Arrange
        var request = new DeleteUserRequestDto(Id: Guid.NewGuid());
        _userRepositoryMock.Setup(r => r.GetByIdAsync(request.Id, default))
            .ReturnsAsync((User)null!);

        var handler = new DeleteUserHandler(
            _unitOfWorkMock.Object,
            _userRepositoryMock.Object,
            _mapperMock.Object);

        // Act
        var result = await handler.Handle(request, default);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_ShouldDeleteUser_WhenUserExists()
    {
        // Arrange
        var request = new DeleteUserRequestDto(Id: Guid.NewGuid());
        var user = new User { Id = request.Id };
        var response = new DeleteUserResponseDto();

        _userRepositoryMock.Setup(r => r.GetByIdAsync(request.Id, default)).ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map<DeleteUserResponseDto>(user)).Returns(response);

        var handler = new DeleteUserHandler(
            _unitOfWorkMock.Object,
            _userRepositoryMock.Object,
            _mapperMock.Object);

        // Act
        var result = await handler.Handle(request, default);

        // Assert
        _userRepositoryMock.Verify(r => r.Delete(user), Times.Once);
        _unitOfWorkMock.Verify(u => u.Commit(default), Times.Once);
        Assert.Equal(response, result);
    }
}