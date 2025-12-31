using SortedTunes.Application.Artists.Commands.DeleteArtist;

namespace SortedTunes.Application.FunctionalTests.Artists.Commands;

using static Testing;

public class DeleteArtistCommandTests : BaseTestFixture
{
    [Test]
    public async Task ShouldDeleteArtist()
    {
        // arrange
        var artist = new Artist { Name = "Artist to Delete" };
        await AddAsync(artist);

        var command = new DeleteArtistCommand(artist.Id);

        // act
        await SendAsync(command);

        // assert
        var deletedArtist = await FindAsync<Artist>(artist.Id);
        deletedArtist.Should().BeNull();
    }

    [Test]
    public async Task ShouldFailWhenArtistNotFound()
    {
        // arrange
        var command = new DeleteArtistCommand(999); // Non-existent artist ID

        // act
        Func<Task> action = async () => await SendAsync(command);

        // assert
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldFailWhenDeletingAlreadyDeletedArtist()
    {
        // arrange
        var artist = new Artist { Name = "Artist to Delete Twice" };
        await AddAsync(artist);

        var command = new DeleteArtistCommand(artist.Id);

        // act
        await SendAsync(command); // First deletion

        Func<Task> action = async () => await SendAsync(command); // Second deletion attempt

        // assert
        await action.Should().ThrowAsync<NotFoundException>();
    }
}
