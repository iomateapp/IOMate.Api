using FluentValidation;
using IOMate.Application.Shared.Behavior;
using MediatR;
using Moq;

public class ValidationBehaviorTests
{
    public record TestRequest(string Name) : IRequest<string>;

    public class TestRequestValidator : AbstractValidator<TestRequest>
    {
        public TestRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    [Fact]
    public async Task Should_Invoke_Next_When_Valid()
    {
        // Arrange
        var validators = new List<IValidator<TestRequest>> { new TestRequestValidator() };
        var behavior = new ValidationBehavior<TestRequest, string>(validators);

        var request = new TestRequest("valid");
        var next = new Mock<RequestHandlerDelegate<string>>();
        next.Setup(n => n.Invoke(It.IsAny<CancellationToken>())).ReturnsAsync("ok");

        // Act
        var result = await behavior.Handle(request, next.Object, CancellationToken.None);

        // Assert
        Assert.Equal("ok", result);
        next.Verify(n => n.Invoke(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Should_Throw_ValidationException_When_Invalid()
    {
        // Arrange
        var validators = new List<IValidator<TestRequest>> { new TestRequestValidator() };
        var behavior = new ValidationBehavior<TestRequest, string>(validators);

        var request = new TestRequest(""); 
        var next = new Mock<RequestHandlerDelegate<string>>();

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            behavior.Handle(request, next.Object, CancellationToken.None));
        next.Verify(n => n.Invoke(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Should_Invoke_Next_When_No_Validators()
    {
        // Arrange
        var validators = new List<IValidator<TestRequest>>();
        var behavior = new ValidationBehavior<TestRequest, string>(validators);

        var request = new TestRequest("");
        var next = new Mock<RequestHandlerDelegate<string>>();
        next.Setup(n => n.Invoke(It.IsAny<CancellationToken>())).ReturnsAsync("ok");

        // Act
        var result = await behavior.Handle(request, next.Object, CancellationToken.None);

        // Assert
        Assert.Equal("ok", result);
        next.Verify(n => n.Invoke(It.IsAny<CancellationToken>()), Times.Once);
    }
}