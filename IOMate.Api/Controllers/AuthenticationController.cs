using IOMate.Application.UseCases.Authentication.Auth;
using IOMate.Application.UseCases.Authentication.Refresh;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IOMate.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        IMediator _mediator;
        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Auth(AuthRequestDto request)
        {
            var userId = await _mediator.Send(request);
            return Ok(userId);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
