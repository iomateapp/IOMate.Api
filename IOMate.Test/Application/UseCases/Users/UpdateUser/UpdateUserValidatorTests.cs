using FluentValidation.TestHelper;
using IOMate.Application.UseCases.Users.UpdateUser;
using Xunit;

public class UpdateUserValidatorTests
{
    private readonly UpdateUserValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var model = new UpdateUserRequestDto("First", "Last", "");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Valid()
    {
        var model = new UpdateUserRequestDto("First", "Last", "test@test.com");
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}