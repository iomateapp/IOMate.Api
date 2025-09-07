using MediatR;

namespace IOMate.Application.UseCases.ClaimGroups.AddClaimToGroup
{
    public record AddClaimToGroupCommand(Guid ClaimGroupId, string Resource, string Action) : IRequest<bool>;
}