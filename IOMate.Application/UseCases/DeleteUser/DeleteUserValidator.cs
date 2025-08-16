using FluentValidation;

namespace IOMate.Application.UseCases.CreateUser
{
    public class DeleteUserValidator :
    AbstractValidator<DeleteUserRequestDto>
    {
        public DeleteUserValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
