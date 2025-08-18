using AutoMapper;
using IOMate.Application.Shared.Exceptions;
using IOMate.Application.UseCases.Users.CreateUser;
using IOMate.Domain.Entities;
using IOMate.Domain.Interfaces;
using Moq;
using Xunit;

public class CreateUserHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new();

    [Fact]
    public async Task Handle_ShouldThrow_WhenEmailAlreadyExists()
    {
        var request = new CreateUserRequestDto("unique@email.com", "Unique", "User", "123");
        _userRepositoryMock.Setup(r => r.GetByEmail(request.Email, default)).ReturnsAsync(new User());

        var handler = new CreateUserHandler(
            _unitOfWorkMock.Object,
            _userRepositoryMock.Object,
            _mapperMock.Object,
            _passwordHasherMock.Object);

        await Assert.ThrowsAsync<BadRequestException>(() =>
            handler.Handle(request, default));
    }

    [Fact]
    public async Task Handle_ShouldCreateUser_WhenEmailIsUnique()
    {
        var request = new CreateUserRequestDto(
            Email: "unique@email.com",
            Password: "123",
            FirstName: "Unique",
            LastName: "User"
        );
        var user = new User();
        var response = new CreateUserResponseDto();

        _userRepositoryMock.Setup(r => r.GetByEmail(request.Email, default))
            .ReturnsAsync((User)null!);
        _mapperMock.Setup(m => m.Map<User>(request)).Returns(user);
        _passwordHasherMock.Setup(h => h.HashPassword(request.Password)).Returns("hashed");
        _mapperMock.Setup(m => m.Map<CreateUserResponseDto>(user)).Returns(response);

        var handler = new CreateUserHandler(
            _unitOfWorkMock.Object,
            _userRepositoryMock.Object,
            _mapperMock.Object,
            _passwordHasherMock.Object);

        var result = await handler.Handle(request, default);

        _userRepositoryMock.Verify(r => r.Add(user), Times.Once);
        _unitOfWorkMock.Verify(u => u.Commit(default), Times.Once);
        Assert.Equal(response, result);
    }
}