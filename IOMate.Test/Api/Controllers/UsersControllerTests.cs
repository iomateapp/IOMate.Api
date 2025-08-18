using IOMate.Api.Controllers;
using IOMate.Application.Shared.Dtos;
using IOMate.Application.UseCases.Users.CreateUser;
using IOMate.Application.UseCases.Users.DeleteUser;
using IOMate.Application.UseCases.Users.GetAllUsers;
using IOMate.Application.UseCases.Users.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class UsersControllerTests
{
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _controller = new UsersController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithUsers()
    {
        var users = new PagedResponseDto<GetAllUsersResponseDto>
        {
            Results = new List<GetAllUsersResponseDto> { new(), new() },
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };

        _mediatorMock.Setup(static m => m.Send(It.IsAny<GetAllUsersRequestDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(users, okResult.Value);
    }

    [Fact]
    public async Task GetAll_UsesDefaultPaging()
    {
        var pagedResponse = new PagedResponseDto<GetAllUsersResponseDto>
        {
            Results = new List<GetAllUsersResponseDto>(),
            TotalCount = 0,
            PageNumber = 1,
            PageSize = 10
        };

        _mediatorMock.Setup(m => m.Send(It.Is<GetAllUsersRequestDto>(r => r.PageNumber == 1 && r.PageSize == 10), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResponse);

        await _controller.GetAll();
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllUsersRequestDto>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Create_ReturnsOk_WithUserId()
    {
        var request = new CreateUserRequestDto("email@test.com", "First", "Last", "pass");
        var userId = Guid.NewGuid();
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateUserRequestDto>(), default))
                     .ReturnsAsync(new CreateUserResponseDto { Id = userId });

        var result = await _controller.Create(request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(userId, ((CreateUserResponseDto)okResult.Value).Id);
    }

    [Fact]
    public async Task Update_ReturnsOk_WithResponse()
    {
        var id = Guid.NewGuid();
        var request = new UpdateUserRequestDto("email@test.com", "First", "Last");
        var response = new UpdateUserResponseDto();
        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.Update(id, request, default);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task Delete_ReturnsBadRequest_WhenIdIsNull()
    {
        var result = await _controller.Delete(null, default);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenUserDeleted()
    {
        var id = Guid.NewGuid();
        var response = new DeleteUserResponseDto();
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteUserRequestDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _controller.Delete(id, default);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(response, okResult.Value);
    }
}