using IOMate.Application.UseCases.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    IMediator _mediator;
    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRequestDto request)
    {
        var userId = await _mediator.Send(request);
        return Ok(userId);
    }

}