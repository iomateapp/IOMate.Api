using FluentValidation;
using IOMate.Application.Resources;
using Microsoft.Extensions.Localization;

namespace IOMate.Application.UseCases.Users.CreateUser
{
    public sealed class CreateUserValidator : AbstractValidator<CreateUserRequestDto>
    {
        public CreateUserValidator(IStringLocalizer<Messages> localizer)
        {
            RuleFor(u => u.Email)
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

            RuleFor(u => u.Password)
                .NotEmpty()
                .WithMessage(localizer["RequiredField", "Password"])
                .MinimumLength(6)
                .WithMessage(localizer["MinLength", "Password", 6]);
        }
    }
}
