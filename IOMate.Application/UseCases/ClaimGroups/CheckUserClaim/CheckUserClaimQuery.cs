using MediatR;

namespace IOMate.Application.UseCases.ClaimGroups.CheckUserClaim
{
    public record CheckUserClaimQuery(Guid UserId, string Resource, string Action) : IRequest<bool>;
}