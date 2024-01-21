using SortedTunes.Application.Artists.Commands.CreateArtist;
using SortedTunes.Application.Artists.Commands.DeleteArtist;
using SortedTunes.Domain.Entities;

namespace SortedTunes.Application.FunctionalTests.Artists.Commands;

using static Testing;

public class DeleteArtistTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidArtistId()
    {
        var command = new DeleteArtistCommand(99);
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteArtist()
    {
        var artistId = await SendAsync(new CreateArtistCommand
        {
            Name = "New Artist"
        });

        await SendAsync(new DeleteArtistCommand(artistId));

        var artist = await FindAsync<Artist>(artistId);

        artist.Should().BeNull();
    }
}
