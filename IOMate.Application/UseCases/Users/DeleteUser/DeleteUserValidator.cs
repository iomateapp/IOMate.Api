using FluentValidation;
using IOMate.Application.Resources;
using Microsoft.Extensions.Localization;

namespace IOMate.Application.UseCases.Users.DeleteUser
{
    public class DeleteUserValidator :
    AbstractValidator<DeleteUserRequestDto>
    {
        public DeleteUserValidator(IStringLocalizer<Messages> localizer)
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(localizer["RequiredField", "Id"]);
        }
    }
}
