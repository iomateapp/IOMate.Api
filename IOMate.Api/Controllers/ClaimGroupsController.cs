using IOMate.Application.UseCases.ClaimGroups.AddClaimToGroup;
using IOMate.Application.UseCases.ClaimGroups.AssignToUser;
using IOMate.Application.UseCases.ClaimGroups.CreateClaimGroup;
using IOMate.Application.UseCases.ClaimGroups.GetUserGroups;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IOMate.Api.Controllers
{
    [Authorize(Policy = "claims:admin")]
    [Route("api/claim-groups")]
    [ApiController]
    public class ClaimGroupsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClaimGroupsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<CreateClaimGroupResponseDto>> Create([FromBody] CreateClaimGroupCommand request)
        {
            var result = await _mediator.Send(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CreateClaimGroupResponseDto>> GetById(Guid id)
        {
            return Ok();
        }

        [HttpPost("{groupId}/claims")]
        public async Task<IActionResult> AddClaim(Guid groupId, [FromBody] AddClaimRequest request)
        {
            var command = new AddClaimToGroupCommand(groupId, request.Resource, request.Action);
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{groupId}/users/{userId}")]
        public async Task<IActionResult> AssignToUser(Guid groupId, Guid userId)
        {
            var command = new AssignClaimGroupToUserCommand(groupId, userId);
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("users/{userId}")]
        public async Task<ActionResult<List<UserClaimGroupResponseDto>>> GetUserGroups(Guid userId)
        {
            var query = new GetUserClaimGroupsQuery(userId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }

    public record AddClaimRequest(string Resource, string Action);
}