using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SortedTunes.Application.Artists.Commands.CreateArtist;
using SortedTunes.Application.Common.Behaviours;
using SortedTunes.Application.Common.Interfaces;

namespace SortedTunes.Application.UnitTests.Common.Behaviours;

public class RequestLoggerTests
{
    private Mock<ILogger<CreateArtistCommand>> _logger = null!;
    private Mock<IUser> _user = null!;
    private Mock<IIdentityService> _identityService = null!;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<CreateArtistCommand>>();
        _user = new Mock<IUser>();
        _identityService = new Mock<IIdentityService>();
    }

    [Test]
    public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
    {
        _user.Setup(x => x.Id).Returns(Guid.NewGuid().ToString());

        var requestLogger = new LoggingBehaviour<CreateArtistCommand>(_logger.Object, _user.Object, _identityService.Object);

        await requestLogger.Process(new CreateArtistCommand { Name = "name" }, new CancellationToken());

        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
    {
        var requestLogger = new LoggingBehaviour<CreateArtistCommand>(_logger.Object, _user.Object, _identityService.Object);

        await requestLogger.Process(new CreateArtistCommand { Name = "title" }, new CancellationToken());

        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Never);
    }
}
