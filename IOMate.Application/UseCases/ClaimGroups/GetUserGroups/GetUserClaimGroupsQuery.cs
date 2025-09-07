using MediatR;

namespace IOMate.Application.UseCases.ClaimGroups.GetUserGroups
{
    public record GetUserClaimGroupsQuery(Guid UserId) : IRequest<List<UserClaimGroupResponseDto>>;
}