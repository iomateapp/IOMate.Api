using FluentValidation.TestHelper;
using IOMate.Application.UseCases.Authentication.Refresh;

public class RefreshValidatorTests : BaseTest
{
    private readonly RefreshTokenRequestDtoValidator _validator;

    public RefreshValidatorTests()
    {
        _validator = new RefreshTokenRequestDtoValidator(Localizer);
    }

    [Fact]
    public void Should_Have_Error_When_RefreshToken_Is_Empty()
    {
        // Arrange
        var model = new RefreshTokenRequestDto
        {
            RefreshToken = ""
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RefreshToken);
    }

    [Fact]
    public void Should_Have_Error_When_RefreshToken_Is_Null()
    {
        // Arrange
        var model = new RefreshTokenRequestDto
        {
            RefreshToken = null
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RefreshToken);
    }

    [Fact]
    public void Should_Not_Have_Error_When_RefreshToken_Is_Valid()
    {
        // Arrange
        var model = new RefreshTokenRequestDto
        {
            RefreshToken = "valid-refresh-token"
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}