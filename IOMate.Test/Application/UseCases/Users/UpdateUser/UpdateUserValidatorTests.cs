using FluentValidation.TestHelper;
using IOMate.Application.UseCases.Users.UpdateUser;

public class UpdateUserValidatorTests : BaseTest
{
    private readonly UpdateUserValidator _validator;

    public UpdateUserValidatorTests()
    {
        _validator = new UpdateUserValidator(Localizer);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        // Arrange
        var model = new UpdateUserRequestDto("First", "Last", "");

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Valid()
    {
        // Arrange
        var model = new UpdateUserRequestDto("First", "Last", "test@test.com");

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}