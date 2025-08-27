using FluentValidation;

namespace IOMate.Application.UseCases.Authentication.Refresh
{
    public class RefreshTokenRequestDtoValidator : AbstractValidator<RefreshTokenRequestDto>
    {
        public RefreshTokenRequestDtoValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("O refresh token é obrigatório.");
        }
    }
}