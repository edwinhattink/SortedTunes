using SortedTunes.Application.Artists.Commands.CreateArtist;
using SortedTunes.Application.Common.Exceptions;
using SortedTunes.Domain.Entities;

namespace SortedTunes.Application.FunctionalTests.Artists.Commands;

using static Testing;

public class CreateArtistTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateArtistCommand() { Name = "" };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireUniqueName()
    {
        await SendAsync(new CreateArtistCommand
        {
            Name = "The Beatles"
        });

        var command = new CreateArtistCommand
        {
            Name = "The Beatles"
        };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreateArtist()
    {
        var userId = await RunAsDefaultUserAsync();

        var command = new CreateArtistCommand
        {
            Name = "Led Zeppelin"
        };

        var id = await SendAsync(command);

        var artist = await FindAsync<Artist>(id);

        artist.Should().NotBeNull();
        artist!.Name.Should().Be(command.Name);
        artist.CreatedBy.Should().Be(userId);
        artist.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
