using IOMate.Application.UseCases.Events.GetEvents;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IOMate.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{entityId:guid}")]
        public async Task<ActionResult<List<object>>> GetEvents(Guid entityId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetEventsRequestDto(entityId), cancellationToken);
            return Ok(result);
        }

    }
}