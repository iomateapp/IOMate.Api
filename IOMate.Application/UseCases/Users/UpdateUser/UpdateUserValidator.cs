using FluentValidation;
using IOMate.Application.Resources;
using Microsoft.Extensions.Localization;

namespace IOMate.Application.UseCases.Users.UpdateUser
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserRequestDto>
    {
        public UpdateUserValidator(IStringLocalizer<Messages> localizer)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(localizer["RequiredField", "Email"])
                .MaximumLength(50)
                .WithMessage(localizer["MaxLength", "Email", 50])
                .EmailAddress()
                .WithMessage(localizer["InvalidEmail"]);

            RuleFor(u => u.FirstName).
                NotEmpty()
                .WithMessage(localizer["RequiredField", "FirstName"])
                .MinimumLength(3)
                .WithMessage(localizer["MinLength", "FirstName", 3])
                .MaximumLength(50)
                .WithMessage(localizer["MaxLength", "FirstName", 50]);

            RuleFor(u => u.LastName)
                .NotEmpty()
                .WithMessage(localizer["RequiredField", "LastName"])
                .MinimumLength(3)
                .WithMessage(localizer["MinLength", "LastName", 3])
                .MaximumLength(50)
                .WithMessage(localizer["MaxLength", "LastName", 50]);
        }
    }
}
