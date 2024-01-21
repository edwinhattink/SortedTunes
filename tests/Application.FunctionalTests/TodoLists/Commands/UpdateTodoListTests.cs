using SortedTunes.Application.Artists.Commands.CreateArtist;
using SortedTunes.Application.Artists.Commands.UpdateArtist;
using SortedTunes.Application.Common.Exceptions;
using SortedTunes.Domain.Entities;

namespace SortedTunes.Application.FunctionalTests.Artists.Commands;

using static Testing;

public class UpdateArtistTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidArtistId()
    {
        var command = new UpdateArtistCommand { Id = 99, Name = "New Name" };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldRequireUniqueName()
    {
        var artistId = await SendAsync(new CreateArtistCommand
        {
            Name = "New Artist"
        });

        await SendAsync(new CreateArtistCommand
        {
            Name = "Other Artist"
        });

        var command = new UpdateArtistCommand
        {
            Id = artistId,
            Name = "Other Artist"
        };

        (await FluentActions.Invoking(() =>
            SendAsync(command))
                .Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.ContainsKey("Name")))
                .And.Errors["Name"].Should().Contain("'Name' must be unique.");
    }

    [Test]
    public async Task ShouldUpdateArtist()
    {
        var userId = await RunAsDefaultUserAsync();

        var artistId = await SendAsync(new CreateArtistCommand
        {
            Name = "New Artist"
        });

        var command = new UpdateArtistCommand
        {
            Id = artistId,
            Name = "Updated Artist Name"
        };

        await SendAsync(command);

        var artist = await FindAsync<Artist>(artistId);

        artist.Should().NotBeNull();
        artist!.Name.Should().Be(command.Name);
        artist.LastModifiedBy.Should().NotBeNull();
        artist.LastModifiedBy.Should().Be(userId);
        artist.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
