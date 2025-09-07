using MediatR;

namespace IOMate.Application.UseCases.ClaimGroups.CreateClaimGroup
{
    public record CreateClaimGroupCommand(string Name, string Description) : IRequest<CreateClaimGroupResponseDto>;
}