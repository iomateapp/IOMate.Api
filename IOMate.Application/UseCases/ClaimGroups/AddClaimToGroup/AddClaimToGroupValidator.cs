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
                .WithMessage(x => $"A combina��o de recurso '{x.Resource}' e a��o '{x.Action}' n�o � v�lida.");
        }
    }
}