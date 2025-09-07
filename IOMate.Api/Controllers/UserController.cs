using IOMate.Application.UseCases.Users.CreateUser;
using IOMate.Application.UseCases.Users.DeleteUser;
using IOMate.Application.UseCases.Users.GetAllUsers;
using IOMate.Application.UseCases.Users.GetUserById;
using IOMate.Application.UseCases.Users.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IOMate.Api.Controllers;

[Authorize]
[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    IMediator _mediator;
    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Policy = "users:read")]
    public async Task<ActionResult<List<GetAllUsersResponseDto>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var request = new GetAllUsersRequestDto(pageNumber, pageSize);
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "users:write")]
    public async Task<IActionResult> Create(CreateUserRequestDto request)
    {
        var userId = await _mediator.Send(request);
        return Ok(userId);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "users:write")]
    public async Task<ActionResult<UpdateUserResponseDto>> Update(Guid id, UpdateUserRequestDto request, CancellationToken cancellationToken)
    {
        var command = new UpdateUserCommand(id, request);
        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "users:delete")]
    public async Task<ActionResult> Delete(Guid? id, CancellationToken cancellationToken)
    {
        if (id is null)
            return BadRequest();

        var deleteUserRequest = new DeleteUserRequestDto(id.Value);

        var response = await _mediator.Send(deleteUserRequest, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "users:read")]
    public async Task<ActionResult<GetUserByIdResponseDto>> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var request = new GetUserByIdRequestDto(id);
        var response = await _mediator.Send(request, cancellationToken);

        if (response == null)
            return NotFound();

        return Ok(response);
    }
}