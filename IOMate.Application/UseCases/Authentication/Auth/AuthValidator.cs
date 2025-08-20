using FluentValidation;
using IOMate.Application.Resources;
using IOMate.Application.UseCases.Authentication.Auth;
using Microsoft.Extensions.Localization;

public class AuthenticationValidator : AbstractValidator<AuthRequestDto>
{
    public AuthenticationValidator(IStringLocalizer<Messages> localizer)
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .WithMessage(localizer["RequiredField", "Email"])
            .EmailAddress()
            .WithMessage(localizer["InvalidEmail"]);

        RuleFor(u => u.Password)
            .NotEmpty()
            .WithMessage(localizer["RequiredField", "Password"])
            .MinimumLength(6)
            .WithMessage(localizer["PasswordMinLength", 6]);
    }
}