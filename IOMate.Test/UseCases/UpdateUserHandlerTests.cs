using AutoMapper;
using IOMate.Application.Shared.Exceptions;
using IOMate.Application.UseCases.UpdateUser;
using IOMate.Domain.Entities;
using IOMate.Domain.Interfaces;
using Moq;
using Xunit;

public class UpdateUserHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    [Fact]
    public async Task Handle_ShouldReturnDefault_WhenUserNotFound()
    {
        var command = new UpdateUserCommand(
            Id: Guid.NewGuid(), 
            Request: new UpdateUserRequestDto(FirstName: "First Name", LastName: "Last", Email: "test@test.com")
         );
        _userRepositoryMock.Setup(r => r.GetByIdAsync(command.Id, default))
            .ReturnsAsync((User)null!);

        var handler = new UpdateUserHandler(
            _unitOfWorkMock.Object,
            _userRepositoryMock.Object,
            _mapperMock.Object);

        var result = await handler.Handle(command, default);

        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenEmailAlreadyExistsForAnotherUser()
    {
        var command = new UpdateUserCommand(
            Id: Guid.NewGuid(),
            Request: new UpdateUserRequestDto(FirstName: "First Name", LastName: "Last", Email: "test@test.com")
        );
        var user = new User { Id = command.Id };
        var existingUser = new User { Id = Guid.NewGuid() };

        _userRepositoryMock.Setup(r => r.GetByIdAsync(command.Id, default)).ReturnsAsync(user);
        _userRepositoryMock.Setup(r => r.GetByEmail(command.Request.Email, default)).ReturnsAsync(existingUser);

        var handler = new UpdateUserHandler(
            _unitOfWorkMock.Object,
            _userRepositoryMock.Object,
            _mapperMock.Object);

        await Assert.ThrowsAsync<BadRequestException>(() =>
            handler.Handle(command, default));
    }

    [Fact]
    public async Task Handle_ShouldUpdateUser_WhenValid()
    {
        var command = new UpdateUserCommand
        (
            Id: Guid.NewGuid(),
            Request: new UpdateUserRequestDto
            (
                Email: "unique@email.com",
                FirstName: "First",
                LastName: "Last"
            )
        );
        var user = new User { Id = command.Id };
        var response = new UpdateUserResponseDto();

        _userRepositoryMock.Setup(r => r.GetByIdAsync(command.Id, default)).ReturnsAsync(user);
        _userRepositoryMock.Setup(r => r.GetByEmail(command.Request.Email, default)).ReturnsAsync((User)null!);
        _mapperMock.Setup(m => m.Map<UpdateUserResponseDto>(user)).Returns(response);

        var handler = new UpdateUserHandler(
            _unitOfWorkMock.Object,
            _userRepositoryMock.Object,
            _mapperMock.Object);

        var result = await handler.Handle(command, default);

        _userRepositoryMock.Verify(r => r.Update(user), Times.Once);
        _unitOfWorkMock.Verify(u => u.Commit(default), Times.Once);
        Assert.Equal(response, result);
    }
}