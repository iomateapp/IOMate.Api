using FluentValidation.TestHelper;
using IOMate.Application.UseCases.Users.CreateUser;

public class CreateUserValidatorTests : ValidationTestBase
{
    private readonly CreateUserValidator _validator;

    public CreateUserValidatorTests()
    {
        _validator = new CreateUserValidator(Localizer);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var model = new CreateUserRequestDto("", "First", "Last", "123456");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Short()
    {
        var model = new CreateUserRequestDto("user@email.com", "First", "Last", "123");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Valid()
    {
        var model = new CreateUserRequestDto("user@email.com", "First", "Last", "123456");
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}