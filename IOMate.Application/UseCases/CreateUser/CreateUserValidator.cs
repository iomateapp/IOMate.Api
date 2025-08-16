using FluentValidation;

namespace IOMate.Application.UseCases.CreateUser
{
    public sealed class CreateUserValidator : AbstractValidator<CreateUserRequestDto>
    {
        public CreateUserValidator()
        {
            RuleFor(u => u.Email).NotEmpty().MaximumLength(50).EmailAddress();
            RuleFor(u => u.FirstName).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(u => u.LastName).NotEmpty().MinimumLength(3).MaximumLength(50);
        }
    }
}
