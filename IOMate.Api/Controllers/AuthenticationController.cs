using IOMate.Application.UseCases.Authentication;
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
        public async Task<IActionResult> Auth(AuthenticationRequestDto request)
        {
            var userId = await _mediator.Send(request);
            return Ok(userId);
        }
    }
}
