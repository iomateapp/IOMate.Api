using FluentValidation;

namespace IOMate.Application.UseCases.UpdateUser
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserRequestDto>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().MaximumLength(50).EmailAddress();
            RuleFor(x => x.FirstName).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.LastName).NotEmpty().MinimumLength(3).MaximumLength(50);
        }
    }
}
