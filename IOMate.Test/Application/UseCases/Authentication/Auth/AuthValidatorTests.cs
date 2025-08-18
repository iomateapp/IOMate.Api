using FluentValidation.TestHelper;
using IOMate.Application.UseCases.Authentication.Auth;
using Xunit;

public class AuthenticationValidatorTests
{
    private readonly AuthenticationValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var model = new AuthenticationRequestDto("", "123456");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var model = new AuthenticationRequestDto("invalid-email", "123456");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Empty()
    {
        var model = new AuthenticationRequestDto("user@email.com", "");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Short()
    {
        var model = new AuthenticationRequestDto("user@email.com", "123");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Valid()
    {
        var model = new AuthenticationRequestDto("user@email.com", "123456");
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}