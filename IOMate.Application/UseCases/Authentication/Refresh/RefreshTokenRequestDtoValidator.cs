using FluentValidation;
using IOMate.Application.Resources;
using Microsoft.Extensions.Localization;

namespace IOMate.Application.UseCases.Authentication.Refresh
{
    public class RefreshTokenRequestDtoValidator : AbstractValidator<RefreshTokenRequestDto>
    {
        public RefreshTokenRequestDtoValidator(IStringLocalizer<Messages> localizer)
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage(localizer["RequiredField", "RefreshToken"]);
        }
    }
}