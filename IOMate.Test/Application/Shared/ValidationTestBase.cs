using IOMate.Application.Resources;
using Microsoft.Extensions.Localization;
using Moq;

public abstract class ValidationTestBase
{
    protected IStringLocalizer<Messages> Localizer { get; }

    protected ValidationTestBase()
    {
        var localizerMock = new Mock<IStringLocalizer<Messages>>();
        localizerMock.Setup(l => l[It.IsAny<string>()])
            .Returns((string key) => new LocalizedString(key, key));
        localizerMock.Setup(l => l[It.IsAny<string>(), It.IsAny<object[]>()])
            .Returns((string key, object[] args) => new LocalizedString(key, key));
        Localizer = localizerMock.Object;
    }
}