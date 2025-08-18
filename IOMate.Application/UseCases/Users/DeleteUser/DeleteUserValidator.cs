using FluentValidation;

namespace IOMate.Application.UseCases.Users.DeleteUser
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
