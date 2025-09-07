using FluentValidation;
using IOMate.Domain.Shared;

namespace IOMate.Application.UseCases.ClaimGroups.AddClaimToGroup
{
    public class AddClaimToGroupValidator : AbstractValidator<AddClaimToGroupCommand>
    {
        public AddClaimToGroupValidator()
        {
            RuleFor(x => x)
                .Must(x => ApplicationClaims.IsValidClaim(x.Resource, x.Action))
                .WithMessage(x => $"A combinação de recurso '{x.Resource}' e ação '{x.Action}' não é válida.");
        }
    }
}