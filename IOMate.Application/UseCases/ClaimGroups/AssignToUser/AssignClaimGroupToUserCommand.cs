using MediatR;

namespace IOMate.Application.UseCases.ClaimGroups.AssignToUser
{
    public record AssignClaimGroupToUserCommand(Guid ClaimGroupId, Guid UserId) : IRequest<bool>;
}