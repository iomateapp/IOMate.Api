using FluentValidation;

namespace IOMate.Application.UseCases.Authentication.Auth
{
    public sealed class AuthenticationValidator : AbstractValidator<AuthenticationRequestDto>
    {
        public AuthenticationValidator()
        {
            RuleFor(u => u.Email).NotEmpty().EmailAddress();
            RuleFor(u => u.Password).NotEmpty().MinimumLength(6);
        }
    }
}
