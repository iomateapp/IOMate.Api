using FluentValidation.TestHelper;
using IOMate.Application.UseCases.Users.DeleteUser;
using System;
using Xunit;

public class DeleteUserValidatorTests
{
    private readonly DeleteUserValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        // Arrange
        var model = new DeleteUserRequestDto(Guid.Empty);

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Valid()
    {
        // Arrange
        var model = new DeleteUserRequestDto(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}