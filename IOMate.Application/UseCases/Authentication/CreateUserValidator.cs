using FluentValidation;
using IOMate.Application.UseCases.Authentication;

namespace IOMate.Application.UseCases.CreateUser
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
